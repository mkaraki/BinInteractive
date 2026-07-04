// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

using System.Collections.Generic;
using BinPlayground.Types.Stencils;

namespace BinPlayground.Types
{
    public interface IPlayable
    {
#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable IDE1006 // Naming Styles

        public IEnumerator<StencilParsedSection> stencil(string stencilName);

        public string ascii { get; }

        public string utf8 { get; }

        public string utf16 { get; }

        public string wide { get; }

        public string utf32 { get; }

        public string hex { get; }

        public IBytesBitmap bitmap { get; }

        public IBytesBitmap bitmap1d { get; }

        public string magic { get; }

        public string file { get; }

        public Bytes le2be(int packedEach = 1);

        public Bytes be2le(int packetEach = 1);

        public IEnumerator<ulong> find(byte[] pattern);

        public ulong? indexOf(byte[] pattern);

        public IEnumerator<ulong> find(string asciiString);


#pragma warning restore IDE0051 // Remove unused private members
#pragma warning restore IDE1006 // Naming Styles
    }
}
