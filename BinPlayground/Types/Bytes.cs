using HeyRed.Mime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BinPlayground.Types.Stencils;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace BinPlayground.Types;

public class Bytes(byte[] bytes, ulong offset = 0) : IReadable
{
    public byte[] _bytes = bytes;

    public ulong offset = offset;

    public byte[] byteArray => _bytes;

    public long length => _bytes.LongLength;

    public Bytes read()
    {
        var newBytes = new byte[length];
        Array.Copy(_bytes, 0, newBytes, 0, length);
        _bytes = Array.Empty<byte>();

        return new Bytes(newBytes, offset);
    }

    public Bytes read(ulong len)
    {
        if (_bytes.Length < (int)len)
        {
            throw new Exception("Requested bytes amount is larger than buffered byte");
        }

        var newBytes = new byte[len];
        Array.Copy(_bytes, 0, newBytes, 0, (long)len);

        var remainCopySize = _bytes.Length - (int)len;
        var remainBytes = new byte[remainCopySize];
        Array.Copy(_bytes, (long)len, remainBytes, 0, remainCopySize);
        _bytes = remainBytes;

        return new Bytes(newBytes, offset);
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

    public string magic
    {
        get
        {
            using var libMagic = new Magic(MagicOpenFlags.MAGIC_NONE);
            return libMagic.Read(_bytes, _bytes.Length);
        }
    }

    public string file => magic;

    public override string ToString() => $"({_bytes.LongLength}) {hex}";

    public Bytes this[Range range]
    {
        get
        {
            var (start, length) = range.GetOffsetAndLength(_bytes.Length);
            var newBytes = new byte[length];
            Array.Copy(_bytes, start, newBytes, 0, length);
            return new Bytes(newBytes, (ulong)start);
        }
    }

    public Bytes skip(int count) => new(_bytes.Skip(count).ToArray());

    public Bytes take(int count) => new(_bytes.Take(count).ToArray());

    // ====================================
    // Search
    // ====================================

    public IEnumerator<ulong> find(byte[] pattern)
    {
        for (int i = 0; i <= _bytes.Length - pattern.Length; i++)
        {
            bool found = true;
            for (int j = 0; j < pattern.Length; j++)
            {
                if (_bytes[i + j] != pattern[j])
                {
                    found = false;
                    break;
                }
            }
            if (found)
            {
                yield return offset + (ulong)i;
            }
        }
    }

    public ulong? indexOf(byte[] pattern)
    {
        for (int i = 0; i <= _bytes.Length - pattern.Length; i++)
        {
            bool found = true;
            for (int j = 0; j < pattern.Length; j++)
            {
                if (_bytes[i + j] != pattern[j])
                {
                    found = false;
                    break;
                }
            }
            if (found)
            {
                return offset + (ulong)i;
            }
        }
        return null;
    }

    public IEnumerator<ulong> find(string asciiString)
    {
        var asciiBytes = Encoding.ASCII.GetBytes(asciiString);
        return find(asciiBytes);
    }

    // ====================================
    // Utilities for 1 byte
    // ====================================

    public ushort? uint8 => BinPlayground.uint8(_bytes);

    public short? int8 => BinPlayground.int8(_bytes);

    // ====================================
    // Utilities for 2 bytes
    // ====================================

    public ushort? uint16le => BinPlayground.uint16le(_bytes);

    public ushort? uint16be => BinPlayground.uint16be(_bytes);

    public short? int16le => BinPlayground.int16le(_bytes);

    public short? int16be => BinPlayground.int16be(_bytes);

    public Half? float16le => BinPlayground.float16le(_bytes);

    public Half? float16be => BinPlayground.float16be(_bytes);

    // ====================================
    // Utilities for 4 bytes
    // ====================================

    public uint? uint32le => BinPlayground.uint32le(_bytes);

    public uint? uint32be => BinPlayground.uint32be(_bytes);

    public int? int32le => BinPlayground.int32le(_bytes);

    public int? int32be => BinPlayground.int32be(_bytes);

    public float? float32le => BinPlayground.float32le(_bytes);

    public float? float32be => BinPlayground.float32be(_bytes);

    // ====================================
    // Utilities for 8 bytes
    // ====================================

    public ulong? uint64le => BinPlayground.uint64le(_bytes);

    public ulong? uint64be => BinPlayground.uint64be(_bytes);

    public long? int64le => BinPlayground.int64le(_bytes);

    public long? int64be => BinPlayground.int64be(_bytes);

    public double? float64le => BinPlayground.float64le(_bytes);

    public double? float64be => BinPlayground.float64be(_bytes);

    // ====================================
    // Utilities for BCD
    // ====================================

    public static ushort bcdToUshort(byte bcd)
    {
        ushort tens = (ushort)(bcd >> 4);
        ushort ones = (ushort)(bcd & 0x0F);

        if (tens > 9 || ones > 9)
        {
            throw new ArgumentException($"Input byte {bcd:X2} is not valid BCD.", nameof(bcd));
        }

        return (ushort)((tens * 10) + ones);
    }

    public ulong bcdLe()
    {
        var bytes = _bytes;
        if (bytes.Length > 9)
        {
            // Due to ulong max is 18 44 67 44 07 37 09 55 16 15
            bytes = bytes.Take(8).ToArray();
        }

        ulong result = 0;
        ulong multiplexer = 1;

        for (int i = 0; i < bytes.Length; i++)
        {
            ushort bcdValue = bcdToUshort(bytes[i]);
            result += (ulong)bcdValue * multiplexer;
            multiplexer *= 100;
        }

        return result;
    }

    public ulong bcdBe()
    {
        var bytes = _bytes;
        if (bytes.Length > 9)
        {
            // Due to ulong max is 18 44 67 44 07 37 09 55 16 15
            bytes = bytes.Take(8).ToArray();
        }

        ulong result = 0;

        for (int i = 0; i < bytes.Length; i++)
        {
            ushort bcdValue = bcdToUshort(bytes[i]);
            result = (result * 100) + (ulong)bcdValue;  
        }

        return result;
    }

    // ====================================
    // Utilities for Hashing
    // ====================================

    public Bytes md5 => new (System.Security.Cryptography.MD5.HashData(_bytes));

    public Bytes sha1 => new (System.Security.Cryptography.SHA1.HashData(_bytes));

    public Bytes sha256 => new (System.Security.Cryptography.SHA256.HashData(_bytes));
    public Bytes sha384 => new (System.Security.Cryptography.SHA384.HashData(_bytes));
    public Bytes sha512 => new (System.Security.Cryptography.SHA512.HashData(_bytes));

    public Bytes sha3_256 => new (System.Security.Cryptography.SHA3_256.HashData(_bytes));
    public Bytes sha3_384 => new (System.Security.Cryptography.SHA3_384.HashData(_bytes));
    public Bytes sha3_512 => new (System.Security.Cryptography.SHA3_512.HashData(_bytes));

    // ====================================
    // End of utilities
    // ====================================
}