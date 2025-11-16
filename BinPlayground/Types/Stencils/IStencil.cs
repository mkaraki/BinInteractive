using System.Collections.Generic;

namespace BinPlayground.Types.Stencils;

public interface IStencil
{
    public IReadable Reader { get; }

    public IEnumerator<StencilParsedSection> Parse();
}