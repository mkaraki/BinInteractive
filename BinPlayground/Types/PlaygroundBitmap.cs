using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BinPlayground.Types
{
    public class PlaygroundBitmap(BinPlayground b) : IBytesBitmap
    {
        public ulong Length { get; } = (ulong)Math.Ceiling(b._file.Length / 3.0);

        public byte[] GetColor(ulong pixel)
        {
            // ToDo: Add caching for performance, as this will be called a lot when rendering the bitmap.

            var targetStart = pixel * 3;
            var byteActualSize = (ulong)b._file.Length;

            if (targetStart >= byteActualSize)
            {
                return [0, 0, 0];
            }

            var startPos = b.pos;
            var color = b.ReadAndRewind(3).ToList();
            b.pos = startPos;

            if (color.Count != 3)
            {
                color.AddRange(Enumerable.Repeat((byte)0, 3 - color.Count));
            }

            return color.ToArray();
        }
    }
}
