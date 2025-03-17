namespace BinPlayground.Types
{
    public interface IBytesBitmap
    {
        public ulong Length { get; }

        public byte[] GetColor(ulong pixel);
    }
}
