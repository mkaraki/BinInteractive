using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace BinPlayground.Types
{
    public static class ByteArrayExtensions
    {
        public static string ascii(this byte[] byteArray)
        {
            return Encoding.ASCII.GetString(byteArray);
        }
    }
}
