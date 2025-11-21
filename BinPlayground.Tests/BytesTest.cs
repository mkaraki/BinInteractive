using System;
using BinPlayground.Types;

namespace BinPlayground.Tests
{
    public class BytesTest
    {
        [Fact]
        public void ReadTest()
        {
            var sampleBytes = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10 };
            
            // Check length
            var bytes = new Bytes(sampleBytes, 0);
            Assert.Equal(17, bytes.length);

            // Read all
            bytes = new Bytes(sampleBytes, 0);
            var read = bytes.read();
            Assert.Equal(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10 }, read._bytes);
            Assert.Equal(new byte[] {}, bytes._bytes);

            // Read specific length
            bytes = new Bytes(sampleBytes, 0);
            read = bytes.read(5);
            Assert.Equal(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04 }, read._bytes);
            Assert.Equal(new byte[] { 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10 }, bytes._bytes);

            // Read beyond length
            bytes = new Bytes(sampleBytes, 0);
            Assert.Throws<Exception>(() =>
            {
                _ = bytes.read(100000);
            });;
        }

        [Fact]
        public void SkipTakeTest()
        {
            var sampleBytes = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10 };
            
            // Skip 0
            var bytes = new Bytes(sampleBytes, 0);
            var res = bytes.skip(0);
            Assert.Equal(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10 }, res._bytes);

            // Skip 13
            bytes = new Bytes(sampleBytes, 0);
            res = bytes.skip(13);
            Assert.Equal(new byte[] { 0x0d, 0x0e, 0x0f, 0x10 }, res._bytes);
            
            // Take 10000
            bytes = new Bytes(sampleBytes, 0);
            res = bytes.take(10000);
            Assert.Equal(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10 }, res._bytes);

            // Take 0
            bytes = new Bytes(sampleBytes, 0);
            res = bytes.take(0);
            Assert.Equal(new byte[] { }, res._bytes);

            // Take 4
            bytes = new Bytes(sampleBytes, 0);
            res = bytes.take(4);
            Assert.Equal(new byte[] { 0x00, 0x01, 0x02, 0x03 }, res._bytes);
        }

        [Fact]
        public void ReverseTest()
        {
            var sampleBytes = new byte[] { 0x01, 0x02, 0x03, 0x04 };
            var bytes = new Bytes(sampleBytes, 0);
            var reversed = bytes.reverse();
            Assert.Equal(new byte[] { 0x04, 0x03, 0x02, 0x01 }, reversed._bytes);
        }

        [Fact]
        public void Le2BeTest()
        {
            var sampleBytes = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 };
            var bytes = new Bytes(sampleBytes, 0);
            var converted = bytes.le2be(2);
            Assert.Equal(new byte[] { 0x02, 0x01, 0x04, 0x03, 0x06, 0x05 }, converted._bytes);

            // Also test be2le here
            var converted2 = bytes.be2le(2);
            Assert.Equal(new byte[] { 0x02, 0x01, 0x04, 0x03, 0x06, 0x05 }, converted2._bytes);
        }

        [Fact]
        public void Le2BeTestWithNonExactPacked()
        {
            var sampleBytes = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 };
            var bytes = new Bytes(sampleBytes, 0);
            var converted = bytes.le2be(2);
            Assert.Equal(new byte[] { 0x02, 0x01, 0x04, 0x03, 0x06, 0x05, 0x00, 0x07 }, converted._bytes);

            // Also test be2le here
            var converted2 = bytes.be2le(2);
            Assert.Equal(new byte[] { 0x02, 0x01, 0x04, 0x03, 0x06, 0x05, 0x00, 0x07 }, converted2._bytes);
        }

        [Fact]
        public void EncodingTest()
        {
            var sampleUtf8Bytes = new byte[] { 0xE3, 0x81, 0x93, 0xE3, 0x82, 0x93, 0xE3, 0x81, 0xAB, 0xE3, 0x81, 0xA1, 0xE3, 0x81, 0xAF };
            var sampleUtf16Bytes = new byte[] { 0x53, 0x30, 0x93, 0x30, 0x6B, 0x30, 0x61, 0x30, 0x6F, 0x30 };
            var sampleUtf32Bytes = new byte[] { 0x53, 0x30, 0x00, 0x00, 0x93, 0x30, 0x00, 0x00, 0x6B, 0x30, 0x00, 0x00, 0x61, 0x30, 0x00, 0x00, 0x6F, 0x30, 0x00, 0x00 };

            // Sample UTF-8 decoding
            var bytes = new Bytes(sampleUtf8Bytes, 0);
            Assert.Equal("こんにちは", bytes.utf8);

            // Sample UTF-16 decoding
            bytes = new Bytes(sampleUtf16Bytes, 0);
            Assert.Equal("こんにちは", bytes.utf16);

            // Sample UTF-16 decoding (wide endpoint)
            bytes = new Bytes(sampleUtf16Bytes, 0);
            Assert.Equal("こんにちは", bytes.wide);

            // Sample UTF-32 decoding
            bytes = new Bytes(sampleUtf32Bytes, 0);
            Assert.Equal("こんにちは", bytes.utf32);
        }

        [Fact]
        public void TestPadRight()
        {
            var sampleByte = new byte[] { 0x01, 0x02 };
            var bytes = new Bytes(sampleByte, 0);
            var padded = bytes.padRight(5);
            Assert.Equal(new byte[] { 0x01, 0x02, 0x00, 0x00, 0x00 }, padded._bytes);
        }

        [Fact]
        public void TestPadRightWithSameSize()
        {
            var sampleByte = new byte[] { 0x01, 0x02, 0x03 }; 
            var bytes = new Bytes(sampleByte, 0);
            var padded = bytes.padRight(3);
            Assert.Equal(new byte[] { 0x01, 0x02, 0x03 }, padded._bytes);
        }

        [Fact]
        public void TestPadRightWithSmallerPadSize()
        {
            var sampleByte = new byte[] { 0x01, 0x02, 0x03, 0x04 };
            var bytes = new Bytes(sampleByte, 0);
            var padded = bytes.padRight(2);
            Assert.Equal(new byte[] { 0x01, 0x02, 0x03, 0x04 }, padded._bytes);
        }

        [Fact]
        public void TestEnumerableByte()
        {
            var sampleByte = new byte[] { 0x01, 0x02, 0x03, 0x04 };
            var bytes = new Bytes(sampleByte);
            var reversed = bytes.Reverse().ToArray();
            Assert.Equal(new byte[] { 0x04, 0x03, 0x02, 0x01 }, reversed);
        }

        [Fact]
        public void TestToString()
        {
            var sampleByte = new byte[] { 0x0A, 0x0B, 0x0C };
            var bytes = new Bytes(sampleByte);
            Assert.Equal("(3) 0A 0B 0C", bytes.ToString());
        }

        [Fact]
        public void TestMagic()
        {
            var sampleBashByte = new byte[]
            {
                0x23, 0x21, 0x2F, 0x75, 0x73, 0x72, 0x2F, 0x62, 0x69, 0x6E, 0x2F, 0x65, 0x6E, 0x76, 0x20, 0x62, 0x61,
                0x73, 0x68, 0x0A
            };
            var bytes = new Bytes(sampleBashByte);
            Assert.Equal("Bourne-Again shell script, ASCII text executable", bytes.magic);

            // Check file method too
            Assert.Equal("Bourne-Again shell script, ASCII text executable", bytes.file);
        }
    }
}
