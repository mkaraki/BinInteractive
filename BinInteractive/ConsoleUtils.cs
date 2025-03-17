using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinPlayground;
using BinPlayground.Types;
using Kokuban;
using Kokuban.AnsiEscape;

namespace BinInteractive
{
    internal static class ConsoleUtils
    {
        public static async Task PrintBytes(Bytes bytes, InteractiveConfig interactiveConfig)
        {
            if (interactiveConfig.HexWidth < 1)
            {
                await Console.Out.WriteLineAsync(bytes.ToString());
                return;
            }
            if (interactiveConfig.ColoredHex)
            {
                await PrintColoredHexLegendAsync();
            }
            for (long i = 0; i < bytes.length; i++)
            {
                var realPos = (ulong)i + bytes.offset;

                if (i % interactiveConfig.HexWidth == 0)
                {
                    await Console.Out.WriteAsync($"{realPos>>16:X4}:{realPos&0xFFFF:X4}:");
                }
                else if (i != 0 && i % 16 == 0)
                {
                    await Console.Out.WriteAsync(" -");
                }

                var value = bytes.byteArray[i];
                if (interactiveConfig.ColoredHex)
                {
                    Console.ResetColor();
                    await Console.Out.WriteAsync(" ");
                    await PrintColoredHexAsync(value);
                }
                else
                {
                    await Console.Out.WriteAsync($" {value:X2}");
                }

                if (i % interactiveConfig.HexWidth == interactiveConfig.HexWidth - 1)
                {
                    await Console.Out.WriteAsync("  ");
                    int startIndex = (int)i - interactiveConfig.HexWidth + 1;
                    int endIndex = (int)i;
                    var b = new Bytes(bytes.byteArray[startIndex..endIndex]);

                    if (!interactiveConfig.OmitAsciiFromHex)
                    {
                        await Console.Out.WriteAsync(b.ascii);
                    }
                    await Console.Out.WriteLineAsync();
                }
            }

            await Console.Out.WriteLineAsync();
        }

        public static async Task PrintColoredHexLegendAsync()
        {
            await Console.Out.WriteAsync("Legend: ");

            await PrintColoredHexAsync(0x00);

            Console.ResetColor();
            await Console.Out.WriteAsync("  ");

            await PrintColoredHexAsync(0x01);
            await Console.Out.WriteAsync("~");
            await PrintColoredHexAsync(0x3F);

            Console.ResetColor();
            await Console.Out.WriteAsync("  ");

            await PrintColoredHexAsync(0x40);
            await Console.Out.WriteAsync("~");
            await PrintColoredHexAsync(0x7F);

            Console.ResetColor();
            await Console.Out.WriteAsync("  ");

            await PrintColoredHexAsync(0x80);
            await Console.Out.WriteAsync("~");
            await PrintColoredHexAsync(0xBF);

            Console.ResetColor();
            await Console.Out.WriteAsync("  ");

            await PrintColoredHexAsync(0xC0);
            await Console.Out.WriteAsync("~");
            await PrintColoredHexAsync(0xFE);

            Console.ResetColor();
            await Console.Out.WriteAsync("  ");

            await PrintColoredHexAsync(0xFF);

            Console.ResetColor();
            await Console.Out.WriteLineAsync();
        }

        public static async Task PrintColoredHexAsync(byte value)
        {
            var color = Chalk.BgBlack.White;

            if (value == 0)
                color = Chalk.BgBlack.Gray;
            else if (value < 0x40)
                color = Chalk.BgGray.White;
            else if (value < 0x80)
                color = Chalk.BgBlue.White;
            else if (value < 0xC0)
                color = Chalk.BgYellow.Black;
            else if (value < 0xFF)
                color = Chalk.BgRed.White;
            else /* value == 0xFF */
                color = Chalk.BgWhite.Black;

            await Console.Out.WriteAsync(color.Render($"{value:X2}"));
        }
    }
}
