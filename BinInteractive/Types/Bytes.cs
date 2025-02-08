using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace BinInteractive.Types
{
    public class Bytes(byte[] bytes, ulong offset = 0)
    {
        public byte[] _bytes = bytes;

        public ulong offset = offset;

        public byte[] byteArray => _bytes;

        public long length => _bytes.LongLength;

        public string ascii
        {
            get
            {
                var ret = string.Empty;
                foreach (var b in _bytes)
                {
                    if (b is >= 0x20 and <= 0x7e)
                    {
                        ret += (char)b;
                    }
                    else
                    {
                        ret += ".";
                    }
                }
                return ret;
            }
        }

        public string utf8 => Encoding.UTF8.GetString(_bytes);
    
        public string utf16 => Encoding.Unicode.GetString(_bytes);
    
        public string wide => Encoding.Unicode.GetString(_bytes);
    
        public string utf32 => Encoding.UTF32.GetString(_bytes);
    
        public string hex => BitConverter.ToString(_bytes).Replace("-", " ");
    
        public BytesBitmap bitmap => new(_bytes);

        public BytesBitmap1D bitmap1d => new(_bytes);
    
        public override string ToString() => $"({_bytes.LongLength}) {hex}";
    }
}
