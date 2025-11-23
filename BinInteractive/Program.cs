using System.Collections;
using BinInteractive;
using BinPlayground;
using BinPlayground.Types;
using BinPlayground.Types.Stencils;
using Kokuban;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

if (args.Length != 1)
{
    await Console.Out.WriteLineAsync("Usage: BinInteractive.exe <file-to-play>");
    Environment.Exit(1);
}

var file = args[0];
if (!File.Exists(file))
{
    await Console.Error.WriteLineAsync("No file exists");
    Environment.Exit(2);
}

var interactiveConfig = new InteractiveConfig();

await using (var fs = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read))
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

        object ret;
        try
        {
            if (res == null)
            {
                var scriptOption = ScriptOptions.Default
                    .WithImports("System", "System.IO", "System.Text", "BinPlayground.Types")
                    .WithReferences(typeof(FileStream).Assembly, typeof(ByteArrayExtensions).Assembly);
                var scr = CSharpScript.Create(command, globalsType: typeof(BinPlayground.BinPlayground), options: scriptOption);
                res = await scr.RunAsync(playground);
            }
            else
                res = await res.ContinueWithAsync(command);
            ret = res.ReturnValue;
        }
        catch (Exception e)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Red;
            await Console.Out.WriteLineAsync(e.Message);
            Console.ResetColor();
            continue;
        }

        await ConsoleUtils.PrintResult(ret, interactiveConfig);
    }
}

exit:
await Console.Out.WriteLineAsync("Bye");
