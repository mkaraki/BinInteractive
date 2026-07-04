using System;
using System.Linq;
using BinPlayground.Types;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace BinPlayground
{
    public static class PlaygroundUtils
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
            return BitConverter.ToUInt16(bytes.Take(2).Reverse().ToArray());
        }

        public static short int16be(byte a, byte b)
        {
            return int16be([a, b]) ?? 0;
        }

        public static short? int16be(byte[] bytes)
        {
            if (bytes.Length < 2) return null;
            return BitConverter.ToInt16(bytes.Take(2).Reverse().ToArray());
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
            return BitConverter.ToUInt32(bytes.Take(4).Reverse().ToArray());
        }

        public static int int32be(byte a, byte b, byte c, byte d)
        {
            return int32be([a, b, c, d]) ?? 0;
        }

        public static int? int32be(byte[] bytes)
        {
            if (bytes.Length < 4) return null;
            return BitConverter.ToInt32(bytes.Take(4).Reverse().ToArray());
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
            return BitConverter.ToUInt64(bytes.Take(8).Reverse().ToArray());
        }

        public static long int64be(byte a, byte b, byte c, byte d, byte e, byte f, byte g, byte h)
        {
            return int64be([a, b, c, d, e, f, g, h]) ?? 0;
        }

        public static long? int64be(byte[] bytes)
        {
            if (bytes.Length < 8) return null;
            return BitConverter.ToInt64(bytes.Take(8).Reverse().ToArray());
        }

        public static Half float16le(byte a, byte b)
        {
            return float16le([a, b]) ?? (Half)0;
        }

        public static Half? float16le(byte[] bytes)
        {
            if (bytes.Length < 2) return null;
            return BitConverter.ToHalf(bytes.Take(2).ToArray());
        }

        public static Half float16be(byte a, byte b)
        {
            return float16be([a, b]) ?? (Half)0;
        }

        public static Half? float16be(byte[] bytes)
        {
            if (bytes.Length < 2) return null;
            return BitConverter.ToHalf(bytes.Take(2).Reverse().ToArray());
        }

        public static float float32le(byte a, byte b, byte c, byte d)
        {
            return float32le([a, b, c, d]) ?? 0;
        }

        public static float? float32le(byte[] bytes)
        {
            if (bytes.Length < 4) return null;
            return BitConverter.ToSingle(bytes.Take(4).ToArray());
        }

        public static float float32be(byte a, byte b, byte c, byte d)
        {
            return float32be([a, b, c, d]) ?? 0;
        }

        public static float? float32be(byte[] bytes)
        {
            if (bytes.Length < 4) return null;
            return BitConverter.ToSingle(bytes.Take(4).Reverse().ToArray());
        }

        public static double float64le(byte a, byte b, byte c, byte d, byte e, byte f, byte g, byte h)
        {
            return float64le([a, b, c, d, e, f, g, h]) ?? 0;
        }

        public static double? float64le(byte[] bytes)
        {
            if (bytes.Length < 8) return null;
            return BitConverter.ToDouble(bytes.Take(8).ToArray());
        }

        public static double float64be(byte a, byte b, byte c, byte d, byte e, byte f, byte g, byte h)
        {
            return float64be([a, b, c, d, e, f, g, h]) ?? 0;
        }

        public static double? float64be(byte[] bytes)
        {
            if (bytes.Length < 8) return null;
            return BitConverter.ToDouble(bytes.Take(8).Reverse().ToArray());
        }

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

        public static string hex(long hex) => hex.ToString("X");

        public static string oct(long oct) => Convert.ToString(oct, 8);

        public static string bin(long bin) => Convert.ToString(bin, 2);

        public static string ascii(byte[] bytes)
        {
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        public static Bytes ascii(string text)
        {
            return new Bytes(System.Text.Encoding.ASCII.GetBytes(text));
        }
        
        public static string utf8(byte[] bytes)
        {
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        public static Bytes utf8(string text)
        {
            return new Bytes(System.Text.Encoding.UTF8.GetBytes(text));
        }

        public static string utf16(byte[] bytes)
        {
            return System.Text.Encoding.Unicode.GetString(bytes);
        }
        
        public static Bytes utf16(string text)
        {
            return new Bytes(System.Text.Encoding.Unicode.GetBytes(text));
        }

        public static string utf32(byte[] bytes)
        {
            return System.Text.Encoding.UTF32.GetString(bytes);
        }
        
        public static Bytes utf32(string text)
        {
            return new Bytes(System.Text.Encoding.UTF32.GetBytes(text));
        }
        

#pragma warning restore IDE0051 // Remove unused private members
#pragma warning restore IDE1006 // Naming Styles
    }
}
