using System;
using System.Linq;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace BinPlayground
{
    public partial class BinPlayground
    {
#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable IDE1006 // Naming Styles

        public static ushort uint8(byte a) => a;

        public static ushort uint8(byte[] bytes)
        {
            if (bytes.Length < 1) return 0;
            return bytes[0];
        }

        public static short int8(byte a) => (sbyte)a;

        public static short int8(byte[] bytes)
        {
            if (bytes.Length < 1) return 0;
            return (sbyte)bytes[0];
        }

        public static ushort uint16le(byte a, byte b)
        {
            return uint16le([a, b]) ?? 0;
        }

        public static ushort? uint16le(byte[] bytes)
        {
            if (bytes.Length < 2) return null;
            return BitConverter.ToUInt16(bytes.Take(2).ToArray());
        }

        public static short int16le(byte a, byte b)
        {
            return int16le([a, b]) ?? 0;
        }

        public static short? int16le(byte[] bytes)
        {
            if (bytes.Length < 2) return null;
            return BitConverter.ToInt16(bytes.Take(2).ToArray());
        }

        public static ushort uint16be(byte a, byte b)
        {
            return uint16be([a, b]) ?? 0;
        }

        public static ushort? uint16be(byte[] bytes)
        {
            if (bytes.Length < 2) return null;
            return BitConverter.ToUInt16(bytes.Reverse().Take(2).ToArray());
        }

        public static short int16be(byte a, byte b)
        {
            return int16be([a, b]) ?? 0;
        }

        public static short? int16be(byte[] bytes)
        {
            if (bytes.Length < 2) return null;
            return BitConverter.ToInt16(bytes.Reverse().Take(2).ToArray());
        }

        public static uint uint32le(byte a, byte b, byte c, byte d)
        {
            return uint32le([a, b, c, d]) ?? 0;
        }

        public static uint? uint32le(byte[] bytes)
        {
            if (bytes.Length < 4) return null;

            return BitConverter.ToUInt32(bytes.Take(4).ToArray());
        }

        public static int int32le(byte a, byte b, byte c, byte d)
        {
            return int32le([a, b, c, d]) ?? 0;
        }

        public static int? int32le(byte[] bytes)
        {
            if (bytes.Length < 4) return null;
            return BitConverter.ToInt32(bytes.Take(4).ToArray());
        }

        public static uint uint32be(byte a, byte b, byte c, byte d)
        {
            return uint32be([a, b, c, d]) ?? 0;
        }

        public static uint? uint32be(byte[] bytes)
        {
            if (bytes.Length < 4) return null;
            return BitConverter.ToUInt32(bytes.Reverse().Take(4).ToArray());
        }

        public static int int32be(byte a, byte b, byte c, byte d)
        {
            return int32be([a, b, c, d]) ?? 0;
        }

        public static int? int32be(byte[] bytes)
        {
            if (bytes.Length < 4) return null;
            return BitConverter.ToInt32(bytes.Reverse().Take(4).ToArray());
        }

        public static ulong uint64le(byte a, byte b, byte c, byte d, byte e, byte f, byte g, byte h)
        {
            return uint64le([a, b, c, d, e, f, g, h]) ?? 0;
        }

        public static ulong? uint64le(byte[] bytes)
        {
            if (bytes.Length < 8) return null;
            return BitConverter.ToUInt64(bytes.Take(8).ToArray());
        }

        public static long int64le(byte a, byte b, byte c, byte d, byte e, byte f, byte g, byte h)
        {
            return int64le([a, b, c, d, e, f, g, h]) ?? 0;
        }

        public static long? int64le(byte[] bytes)
        {
            if (bytes.Length < 8) return null;
            return BitConverter.ToInt64(bytes.Take(8).ToArray());
        }

        public static ulong uint64be(byte a, byte b, byte c, byte d, byte e, byte f, byte g, byte h)
        {
            return uint64be([a, b, c, d, e, f, g, h]) ?? 0;
        }

        public static ulong? uint64be(byte[] bytes)
        {
            if (bytes.Length < 8) return null;
            return BitConverter.ToUInt64(bytes.Reverse().Take(8).ToArray());
        }

        public static long int64be(byte a, byte b, byte c, byte d, byte e, byte f, byte g, byte h)
        {
            return int64be([a, b, c, d, e, f, g, h]) ?? 0;
        }

        public static long? int64be(byte[] bytes)
        {
            if (bytes.Length < 8) return null;
            return BitConverter.ToInt64(bytes.Reverse().Take(8).ToArray());
        }

#pragma warning restore IDE0051 // Remove unused private members
#pragma warning restore IDE1006 // Naming Styles
    }
}
