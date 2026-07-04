using BinPlayground.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BinPlayground.Types.Stencils;
using HeyRed.Mime;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace BinPlayground
{
    public class BinPlayground(Stream file, InteractiveConfig conf) : IPlayable, IReadable
    {
        public const int DEFAULT_READ_BUFFER_SIZE = 4096;

        public Bytes ReadAndRewind()
        {
            var startPos = pos;
            try
            {
                return read();
            }
            finally
            {
                pos = startPos;
            }
        }

        public Bytes ReadAndRewind(ulong len)
        {
            var startPos = pos;
            try
            {
                return read(len);
            }
            finally
            {
                pos = startPos;
            }
        }

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

        public Bytes read() => read(true);

        public Bytes read(bool absolutePos)
        {
            ulong startPos = 0;
            if (absolutePos) startPos += (ulong)pos;

            var buffer = new byte[length - pos];
            _file.ReadExactly(buffer, 0, buffer.Length);

            var bytes = new Bytes(buffer, startPos);
            return bytes;
        }

        public Bytes read(ulong len) => read((int)len, true);

        public Bytes read(int len, bool absolutePos)
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
        public IEnumerator<StencilParsedSection> stencil(string stencilName)
        {
            IStencil stencil;
            switch (stencilName)
            {
                case "png":
                    stencil = new PngStencil(this);
                    break;
                default:
                    throw new Exception($"Stencil '{stencilName}' is not supported.");
            }

            return stencil.Parse();
        }

        public string ascii => PlaygroundUtils.ascii(ReadAndRewind());
        public string utf8 => PlaygroundUtils.utf8(ReadAndRewind());
        public string utf16 => PlaygroundUtils.utf16(ReadAndRewind());
        public string wide => PlaygroundUtils.utf16(ReadAndRewind());
        public string utf32 => PlaygroundUtils.utf32(ReadAndRewind());
        public string hex => BitConverter.ToString(ReadAndRewind()).Replace("-", " ");

        public IBytesBitmap bitmap => new PlaygroundBitmap(this);
        public IBytesBitmap bitmap1d => new PlaygroundBitmap1D(this);

        public string magic
        {
            get
            {
                using var libMagic = new Magic(MagicOpenFlags.MAGIC_NONE);
                // Magic library will read into MemoryStream internally when pass Stream.
                // So, we decided to pass bytes object instead of Stream.
                var bytes = ReadAndRewind();
                return libMagic.Read(bytes._bytes, bytes._bytes.Length);
            }
        }

        public string file => magic;

        public Bytes le2be(int packedEach = 1)
        {
            return ReadAndRewind().le2be();
        }

        public Bytes be2le(int packetEach = 1) => le2be(packetEach);

        public IEnumerator<ulong> find(byte[] pattern)
        {
            var startPos = pos;

            var patternLength = pattern.Length;

            if (patternLength < 1)
            {
                yield break;
            }

            var firstByte = pattern[0];

            var firstMatchStartLocations = new List<int>();

            var buf = new byte[DEFAULT_READ_BUFFER_SIZE];
            var readAmount = 0;
            var currentBase = 0;
            while ((readAmount = _file.Read(buf, 0, buf.Length)) > 0)
            {
                var lastIndexOf = 0;
                while ((lastIndexOf = Array.IndexOf(buf, firstByte, lastIndexOf, readAmount)) != -1)
                {
                    // Process the found pattern
                    if (patternLength == 1)
                        yield return (ulong)(currentBase + lastIndexOf);
                    else /* patternLength > 1 */
                        firstMatchStartLocations.Add(currentBase + lastIndexOf);

                    lastIndexOf++; // Move past the last found index to continue searching
                }
                currentBase += readAmount;
            }

            if (patternLength == 1)
            {
                pos = startPos;
                yield break;
            }

            // Check remain bytes

            foreach (var startLocation in firstMatchStartLocations)
            {
                _file.Seek(startLocation, SeekOrigin.Begin);
                var buffer = read((ulong)patternLength);
                if (buffer.length == patternLength && buffer._bytes.SequenceEqual(pattern))
                {
                    yield return (ulong)startLocation;
                }
            }

            pos = startPos;
        }

        public ulong? indexOf(byte[] pattern)
        {
            var startPos = pos;

            var patternLength = pattern.Length;

            if (patternLength < 1)
            {
                return null;
            }

            var firstByte = pattern[0];

            var buf = new byte[DEFAULT_READ_BUFFER_SIZE];
            var readAmount = 0;
            var currentBase = 0;
            while ((readAmount = _file.Read(buf, 0, buf.Length)) > 0)
            {
                var lastIndexOf = 0;
                var searchStartPos = pos;
                while ((lastIndexOf = Array.IndexOf(buf, firstByte, lastIndexOf, readAmount)) != -1)
                {
                    pos = currentBase + lastIndexOf;
                    var searchReadBuf = read((ulong)patternLength);
                    if (searchReadBuf.length == patternLength && searchReadBuf._bytes.SequenceEqual(pattern))
                    {
                        pos = startPos;
                        return (ulong)(currentBase + lastIndexOf);
                    }

                    lastIndexOf++; // Move past the last found index to continue searching
                }
                pos = searchStartPos;
                currentBase += readAmount;
            }

            pos = startPos;
            return null;
        }

        public IEnumerator<ulong> find(string asciiString)
        {
            var pattern = PlaygroundUtils.ascii(asciiString);
            return find(pattern);
        }
    }
        
#pragma warning restore IDE0051 // Remove unused private members
#pragma warning restore IDE1006 // Naming Styles
}