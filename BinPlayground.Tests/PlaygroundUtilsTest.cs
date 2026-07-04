using BinPlayground;

namespace BinPlayground.Tests
{
    public class PlaygroundUtilsTest
    {
        [Fact]
        public void Conversions_Primitives()
        {
            // uint8
            Assert.Equal((ushort)255, PlaygroundUtils.uint8((byte)255));
            Assert.Equal((ushort)1, PlaygroundUtils.uint8(new byte[] { 1 }));

            // int8 (signed 8-bit)
            Assert.Equal((short)-1, PlaygroundUtils.int8((byte)255));
            Assert.Equal((short)0, PlaygroundUtils.int8(new byte[] { 0 }));
        }

        [Fact]
        public void Endianness_IntegerConversions()
        {
            // uint16 little-endian: bytes {0x34, 0x12} => 0x1234
            Assert.Equal((ushort)0x1234, PlaygroundUtils.uint16le(new byte[] { 0x34, 0x12 }) ?? 0);

            // uint16 big-endian: bytes {0x12, 0x34} => 0x1234
            Assert.Equal((ushort)0x1234, PlaygroundUtils.uint16be(new byte[] { 0x12, 0x34 }) ?? 0);

            // int16 big-endian negative: {0xFF,0xFF} => -1
            Assert.Equal((short)-1, PlaygroundUtils.int16be(new byte[] { 0xFF, 0xFF }) ?? 0);

            // uint32 little-endian: {1,0,0,0} => 1
            Assert.Equal((uint)1, PlaygroundUtils.uint32le(new byte[] { 1, 0, 0, 0 }) ?? 0);

            // int32 big-endian: all 0xFF => -1
            Assert.Equal(-1, PlaygroundUtils.int32be(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF }) ?? 0);

            // uint64 little-endian: {1,0,0,0,0,0,0,0} => 1
            Assert.Equal((ulong)1, PlaygroundUtils.uint64le(new byte[] { 1, 0, 0, 0, 0, 0, 0, 0 }) ?? 0UL);

            // uint64 big-endian: all 0xFF => ulong.MaxValue
            Assert.Equal(ulong.MaxValue,
                PlaygroundUtils.uint64be(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }));
        }

        [Fact]
        public void FloatingPointConversions()
        {
            // float16 (Half) little-endian bytes for 1.0 => 0x3C00 -> [0x00, 0x3C]
            Assert.Equal((Half)1.0, PlaygroundUtils.float16le(new byte[] { 0x00, 0x3C }) ?? (Half)0);

            // float16 (Half) big-endian bytes for 1.0 => 0x3C00 -> [0x3C, 0x00]
            Assert.Equal((Half)1.0, PlaygroundUtils.float16be(new byte[] { 0x3C, 0x00 }) ?? (Half)0);

            // float32 little-endian for 1.0 => bytes {0x00,0x00,0x80,0x3F}
            Assert.Equal(1.0f, PlaygroundUtils.float32le(new byte[] { 0x00, 0x00, 0x80, 0x3F }) ?? 0f);

            // float32 big-endian for 1.0 => bytes {0x3F,0x80,0x00,0x00}
            Assert.Equal(1.0f, PlaygroundUtils.float32be(new byte[] { 0x3F, 0x80, 0x00, 0x00 }) ?? 0f);

            // float64 little-endian for 1.0 => 0x3FF0000000000000 -> {0x00,0x00,0x00,0x00,0x00,0x00,0xF0,0x3F}
            Assert.Equal(1.0, PlaygroundUtils.float64le(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0x3F }) ?? 0.0);

            // float64 big-endian for 1.0 => 0x3FF0000000000000 -> {0x3F,0xF0,0x00,0x00,0x00,0x00,0x00,0x00}
            Assert.Equal(1.0, PlaygroundUtils.float64be(new byte[] { 0x3F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }) ?? 0.0);
        }

        [Fact]
        public void TestBcdToUShortTest()
        {
            Assert.Equal(12, PlaygroundUtils.bcdToUshort(0x12));

            Assert.Throws<ArgumentException>(() => PlaygroundUtils.bcdToUshort(0x1A));
            Assert.Throws<ArgumentException>(() => PlaygroundUtils.bcdToUshort(0xA1));
        }

        [Fact]
        public void FormatHelpers()
        {
            // hex
            Assert.Equal("FF", PlaygroundUtils.hex(255));

            // octal: 9 -> 11 (base 8)
            Assert.Equal("11", PlaygroundUtils.oct(9));

            // binary: 5 -> 101
            Assert.Equal("101", PlaygroundUtils.bin(5));
        }
    }
}
