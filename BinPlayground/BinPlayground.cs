﻿using BinPlayground.Types;
using System;
using System.IO;
using System.Linq;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace BinPlayground
{
    public partial class BinPlayground(Stream file, InteractiveConfig conf)
    {
#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable IDE1006 // Naming Styles
        public readonly Stream _file = file;

        public readonly InteractiveConfig config = conf;

        public long pos
        {
            get => _file.Position;
            set => _file.Seek(value, SeekOrigin.Begin);
        }

        public long length => _file.Length;

        public void seek(long offset)
        {
            pos += offset;
        }

        public Bytes read(bool absolutePos = true)
        {
            ulong startPos = 0;
            if (absolutePos) startPos += (ulong)pos;

            var buffer = new byte[length - pos];
            _file.ReadExactly(buffer, 0, buffer.Length);

            var bytes = new Bytes(buffer, startPos);
            return bytes;
        }

        public Bytes read(int len, bool absolutePos = true)
        {
            ulong startPos = 0;
            if (absolutePos) startPos += (ulong)pos;

            var buffer = new byte[len];
            var readAmount = _file.Read(buffer, 0, len);
            if (readAmount < len)
            {
                Array.Resize(ref buffer, readAmount);
            }

            var bytes = new Bytes(buffer, startPos);
            return bytes;
        }
#pragma warning restore IDE0051 // Remove unused private members
#pragma warning restore IDE1006 // Naming Styles
    }
}
