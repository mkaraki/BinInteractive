using System;
// ReSharper disable UseUtf8StringLiteral

namespace BinPlayground.Types
{
    public class BytesBitmap : IBytesBitmap
    {
        internal readonly byte[] _bytes;

        public BytesBitmap(Bytes bytes)
        {
            _bytes = bytes.byteArray;
        }

        public BytesBitmap(byte[] bytes)
        {
            _bytes = bytes;
        }

        public ulong Length => (ulong)Math.Ceiling(_bytes.LongLength / 3.0);

        public byte[] GetColor(ulong pixel)
        {
            var targetStart = pixel * 3;
            var byteActualSize = (ulong)_bytes.LongLength;

            if (targetStart >= byteActualSize)
            {
                return [0, 0, 0];
            }

            var color = new byte[3];

            for (ulong i = 0; i < 3; i++)
            {
                if (targetStart + i < byteActualSize)
                {
                    color[i] = _bytes[targetStart + i];
                }
                else
                {
                    color[i] = 0;
                }
            }

            return color;
        }
    }
}
