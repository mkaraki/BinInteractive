namespace BinPlayground.Types
{
    public class PlaygroundBitmap1D(BinPlayground b) : IBytesBitmap
    {
        public ulong Length { get; } = (ulong)b._file.Length;

        public byte[] GetColor(ulong pixel)
        {
            // ToDo: Add caching for performance, as this will be called a lot when rendering the bitmap.

            if (pixel >= Length)
            {
                return [0, 0, 0];
            }

            var startPos = b.pos;
            var byteInfo = b.ReadAndRewind(1)._bytes;
            b.pos = startPos;

            return [byteInfo[0], byteInfo[0], byteInfo[0]];
        }
    }
}
