using BinInteractive;
using BinPlayground;
using BinPlayground.Types;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

var interactiveConfig = new InteractiveConfig();

Stream fs;

if (args.Length != 1)
{
    await Console.Out.WriteLineAsync("No file specified, using empty memory stream");

    fs = new MemoryStream();
}
else
{
    var file = args[0];
    if (!File.Exists(file))
    {
        await Console.Error.WriteLineAsync("No file exists");
        Environment.Exit(2);
    }
    
    fs = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read);
}

CancellationTokenSource? cts = null;

Console.CancelKeyPress += (sender, eventArgs) =>
{
    if (cts != null && !cts.Token.IsCancellationRequested)
    {
        eventArgs.Cancel = true;
        Console.WriteLine();
        Console.ResetColor();
        Console.WriteLine("! Perform cancellation. Re-type Ctrl+C to force exit.");
        Console.WriteLine();
        cts.Cancel();
    }
};

try
{
    var playground = new BinPlayground.BinPlayground(fs, interactiveConfig);
    ulong commandNum = 0;

    ScriptState<object>? res = null;

    while (true)
    {

        await Console.Out.WriteAsync($"{commandNum++}::{playground.pos:X8}> ");
        var command = (await Console.In.ReadLineAsync())?.Trim();
        switch (command)
        {
            case null:
            case "":
                continue;
            case "exit":
            case "bye":
            case "quit":
                goto exit;
        }

        cts = new CancellationTokenSource();
        try
        {
            if (res == null)
            {
                var scriptOption = ScriptOptions.Default
                    .WithImports("System", "System.IO", "System.Text", "BinPlayground.Types",
                        "BinPlayground.PlaygroundUtils", "BinPlayground.Extensions", "System.Collections.Generic",
                        "System.Linq")
                    .WithReferences(
                        typeof(FileStream).Assembly,
                        typeof(ByteArrayExtensions).Assembly
                    );
                var scr = CSharpScript.Create(command, globalsType: typeof(BinPlayground.BinPlayground),
                    options: scriptOption);
                res = await scr.RunAsync(playground, cts.Token);
            }
            else
                res = await res.ContinueWithAsync(command, null, cts.Token);

            object? ret = res.ReturnValue;

            await ConsoleUtils.PrintResult(ret, interactiveConfig, cts.Token);
        }
        catch (Exception e)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Red;
            await Console.Out.WriteLineAsync(e.Message);
            Console.ResetColor();
        }
        finally
        {
            cts?.Dispose();
            cts = null;
        }
    }
}
finally
{
    await fs.DisposeAsync();
}

exit:
await Console.Out.WriteLineAsync("Bye");
