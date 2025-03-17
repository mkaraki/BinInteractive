// ReSharper disable UseUtf8StringLiteral

namespace BinPlayground.Types
{
    public class BytesBitmap1D(byte[] bytes) : IBytesBitmap
    {
        public ulong Length => (ulong)bytes.LongLength;

        public byte[] GetColor(ulong pixel)
        {
            if (pixel >= Length)
            {
                return [0, 0, 0];
            }
            return [bytes[pixel], bytes[pixel], bytes[pixel]];
        }
    }
}
