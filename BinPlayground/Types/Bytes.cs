using HeyRed.Mime;
using System;
using System.Text;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace BinPlayground.Types;

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
    // End of utilities
    // ====================================
}