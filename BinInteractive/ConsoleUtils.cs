using BinPlayground;
using BinPlayground.Types;
using Kokuban;

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
                await PrintColoredHexLegendAsync(interactiveConfig);
            }
            for (long i = 0; i < bytes.length; i++)
            {
                var realPos = (ulong)i + bytes.offset;

                if (i % interactiveConfig.HexWidth == 0)
                {
                    await Console.Out.WriteAsync($"{realPos >> 16:X4}:{realPos & 0xFFFF:X4}:");
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
                    await PrintColoredHexAsync(value, interactiveConfig);
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
                    var b = new Bytes(bytes.byteArray[startIndex..(endIndex + 1)]);

                    if (!interactiveConfig.OmitAsciiFromHex)
                    {
                        await Console.Out.WriteAsync(b.ascii);
                    }
                    await Console.Out.WriteLineAsync();
                }
            }

            await Console.Out.WriteLineAsync();
        }

        public static async Task PrintColoredHexLegendAsync(InteractiveConfig interactiveConfig)
        {
            await Console.Out.WriteAsync("Legend: ");

            await PrintColoredHexAsync(0x00, interactiveConfig);

            Console.ResetColor();
            await Console.Out.WriteAsync("  ");


            if (interactiveConfig.AsciiColoredHex)
            {
                await PrintColoredHexAsync(0x01, interactiveConfig);
                await Console.Out.WriteAsync("~");
                await PrintColoredHexAsync(0x1f, interactiveConfig);

                Console.ResetColor();
                await Console.Out.WriteAsync("  ");

                await PrintColoredHexAsync(0x20, interactiveConfig);
                await Console.Out.WriteAsync("~");
                await PrintColoredHexAsync(0x7e, interactiveConfig);

                Console.ResetColor();
                await Console.Out.WriteAsync("  ");

                await PrintColoredHexAsync(0x7f, interactiveConfig);

                Console.ResetColor();
                await Console.Out.WriteAsync("  ");

                await PrintColoredHexAsync(0x80, interactiveConfig);
                await Console.Out.WriteAsync("~");

                Console.ResetColor();
                await Console.Out.WriteLineAsync();
            }
            else
            {
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

            await Console.Out.WriteLineAsync();
            await Console.Out.WriteLineAsync("     ADDR: _0 _1 _2 _3 _4 _5 _6 _7 _8 _9 _A _B _C _D _E _F");
        }

        public static async Task PrintColoredHexAsync(byte value, InteractiveConfig? interactiveConfig = null)
        {
            var color = Chalk.BgBlack.White;

            if (interactiveConfig is { AsciiColoredHex: true })
            {
                if (value == 0)
                    color = Chalk.BgBlack.Gray;
                else if (value < 0x20 || value == 0x7f) /* Controls */
                    color = Chalk.BgGray.White;
                else if (value < 0x7f) /* ASCII Range */
                    color = Chalk.BgBlue.White;
                else /* value >= 0x80 */
                    color = Chalk.BgWhite.Black;
            }
            else
            {
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
            }

            await Console.Out.WriteAsync(color.Render($"{value:X2}"));
        }
    }
}
