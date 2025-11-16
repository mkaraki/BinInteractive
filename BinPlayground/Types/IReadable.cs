// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace BinPlayground.Types
{
    public interface IReadable
    {
        public Bytes read();

        public Bytes read(ulong len);
    }
}
