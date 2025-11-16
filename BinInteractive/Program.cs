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

        switch (ret)
        {
            case null:
                continue;
            case Bytes bytes:
                await ConsoleUtils.PrintBytes(bytes, interactiveConfig);
                break;
            case IBytesBitmap bitmap:
                {
                    for (ulong i = 0; i < bitmap.Length; i++)
                    {
                        if (interactiveConfig.BitmapWidth > 0 && i % (uint)interactiveConfig.BitmapWidth == 0)
                        {
                            Console.ResetColor();
                            await Console.Out.WriteLineAsync();
                        }

                        var color = bitmap.GetColor(i);
                        var bgRgb = Chalk.BgRgb(color[0], color[1], color[2]);
                        await Console.Out.WriteAsync(bgRgb.Render(" "));
                        Console.ResetColor();
                    }

                    await Console.Out.WriteLineAsync();
                    Console.ResetColor();
                    break;
                }
            case IEnumerator<ulong> enumerator:
                {
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            var i = enumerator.Current;
                            await Console.Out.WriteLineAsync($"0x{i:X8}");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Red;
                        await Console.Out.WriteLineAsync(e.Message);
                        Console.ResetColor();
                    }
                    break;
                }
            case IEnumerator<StencilParsedSection> enumerator:
                {
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            var section = enumerator.Current;
                            await Console.Out.WriteAsync($"0x{section.Address:X8}: ");
                            foreach (byte b in section.Data)
                            {
                                await Console.Out.WriteAsync($"{b:X2} ");
                            }
                            await Console.Out.WriteLineAsync($": {section.Description} ({section.ParsedValue})");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Red;
                        await Console.Out.WriteLineAsync(e.Message);
                        Console.ResetColor();
                    }
                    break;
                }
            default:
                {
                    var retStr = ret.ToString();
                    if (!string.IsNullOrEmpty(retStr))
                    {
                        await Console.Out.WriteLineAsync(retStr);
                    }
                    break;
                }
        }
    }
}

exit:
await Console.Out.WriteLineAsync("Bye");
