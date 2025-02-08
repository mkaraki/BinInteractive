using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinInteractive.Types
{
    public interface IBytesBitmap
    {
        public ulong Length { get; }

        public byte[] GetColor(ulong pixel);
    }
}
