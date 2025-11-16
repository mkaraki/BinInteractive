using BinPlayground.Types;

namespace BinPlayground.Tests
{
    public class PlaygroundUtilsTest
    {
        [Fact]
        public void Conversions_Primitives()
        {
            // uint8
            Assert.Equal((ushort)255, BinPlayground.uint8((byte)255));
            Assert.Equal((ushort)1, BinPlayground.uint8(new byte[] { 1 }));

            // int8 (signed 8-bit)
            Assert.Equal((short)-1, BinPlayground.int8((byte)255));
            Assert.Equal((short)0, BinPlayground.int8(new byte[] { 0 }));
        }

        [Fact]
        public void Endianness_IntegerConversions()
        {
            // uint16 little-endian: bytes {0x34, 0x12} => 0x1234
            Assert.Equal((ushort)0x1234, BinPlayground.uint16le(new byte[] { 0x34, 0x12 }) ?? 0);

            // uint16 big-endian: bytes {0x12, 0x34} => 0x1234
            Assert.Equal((ushort)0x1234, BinPlayground.uint16be(new byte[] { 0x12, 0x34 }) ?? 0);

            // int16 big-endian negative: {0xFF,0xFF} => -1
            Assert.Equal((short)-1, BinPlayground.int16be(new byte[] { 0xFF, 0xFF }) ?? 0);

            // uint32 little-endian: {1,0,0,0} => 1
            Assert.Equal((uint)1, BinPlayground.uint32le(new byte[] { 1, 0, 0, 0 }) ?? 0);

            // int32 big-endian: all 0xFF => -1
            Assert.Equal(-1, BinPlayground.int32be(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF }) ?? 0);

            // uint64 little-endian: {1,0,0,0,0,0,0,0} => 1
            Assert.Equal((ulong)1, BinPlayground.uint64le(new byte[] { 1, 0, 0, 0, 0, 0, 0, 0 }) ?? 0UL);

            // uint64 big-endian: all 0xFF => ulong.MaxValue
            Assert.Equal(ulong.MaxValue,
                BinPlayground.uint64be(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }));
        }

        [Fact]
        public void FloatingPointConversions()
        {
            // float16 (Half) little-endian bytes for 1.0 => 0x3C00 -> [0x00, 0x3C]
            Assert.Equal((Half)1.0, BinPlayground.float16le(new byte[] { 0x00, 0x3C }) ?? (Half)0);

            // float16 (Half) big-endian bytes for 1.0 => 0x3C00 -> [0x3C, 0x00]
            Assert.Equal((Half)1.0, BinPlayground.float16be(new byte[] { 0x3C, 0x00 }) ?? (Half)0);

            // float32 little-endian for 1.0 => bytes {0x00,0x00,0x80,0x3F}
            Assert.Equal(1.0f, BinPlayground.float32le(new byte[] { 0x00, 0x00, 0x80, 0x3F }) ?? 0f);

            // float32 big-endian for 1.0 => bytes {0x3F,0x80,0x00,0x00}
            Assert.Equal(1.0f, BinPlayground.float32be(new byte[] { 0x3F, 0x80, 0x00, 0x00 }) ?? 0f);

            // float64 little-endian for 1.0 => 0x3FF0000000000000 -> {0x00,0x00,0x00,0x00,0x00,0x00,0xF0,0x3F}
            Assert.Equal(1.0, BinPlayground.float64le(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0x3F }) ?? 0.0);

            // float64 big-endian for 1.0 => 0x3FF0000000000000 -> {0x3F,0xF0,0x00,0x00,0x00,0x00,0x00,0x00}
            Assert.Equal(1.0, BinPlayground.float64be(new byte[] { 0x3F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }) ?? 0.0);
        }

        [Fact]
        public void FormatHelpers()
        {
            // hex
            Assert.Equal("FF", BinPlayground.hex(255));

            // octal: 9 -> 11 (base 8)
            Assert.Equal("11", BinPlayground.oct(9));

            // binary: 5 -> 101
            Assert.Equal("101", BinPlayground.bin(5));
        }
    }
}
