using System;
using System.Collections.Generic;

namespace BinPlayground.Types.Stencils
{
    internal class PngStencil(IReadable reader) : IStencil
    {
        private static readonly byte[] PNG_SIGNATURE = [0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a];

        public IReadable Reader { get; private set; } = reader;

        public IEnumerator<StencilParsedSection> Parse()
        {
            var pngSignature = Reader.read(8);

            if (!pngSignature._bytes.SequenceEqual(PNG_SIGNATURE))
            {
                yield return new StencilParsedSection()
                {
                    Address = pngSignature.offset,
                    Description = "PNG Signature",
                    Data = pngSignature._bytes,
                    ParsedValue = "Invalid",
                };
                throw new Exception("Not a valid PNG file.");
            }

            yield return new StencilParsedSection()
            {
                Address = pngSignature.offset,
                Description = "PNG Signature",
                Data = pngSignature._bytes,
                ParsedValue = "Valid",
            };
        }
    }
}
