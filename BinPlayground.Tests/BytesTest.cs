using BinPlayground.Extensions;
using BinPlayground.Types;

namespace BinPlayground.Tests
{
    public class BytesTest
    {
        [Fact]
        public void InitializeTest()
        {
            // No args
            var init = new Bytes();
            Assert.Equal(new byte[] {}, init._bytes);
            Assert.Equal(0UL, init.offset);

            // Collection initializer
            init = new Bytes() { 0x01, 0x02, 0x03, 0x04, 0x05 };
            Assert.Equal(new byte[] {0x01, 0x02, 0x03, 0x04, 0x05}, init._bytes);
            Assert.Equal(0UL, init.offset);

            // No offset
            init = new Bytes([0x01, 0x02, 0x03, 0x04, 0x05]);
            Assert.Equal(new byte[] {0x01, 0x02, 0x03, 0x04, 0x05}, init._bytes);
            Assert.Equal(0UL, init.offset);

            // With offset
            init = new Bytes([0x01, 0x02, 0x03, 0x04, 0x05], 0x100);
            Assert.Equal(new byte[] {0x01, 0x02, 0x03, 0x04, 0x05}, init._bytes);
            Assert.Equal(0x100UL, init.offset);
        }

        [Fact]
        public void TestClone()
        {
            Bytes originalBytes;
            Bytes clonedBytes;

            var offset = 0UL;
            var bytes = new byte[] { 0x0a, 0x0b, 0x0c };
            originalBytes = new Bytes(bytes, offset);
            clonedBytes = originalBytes.Clone();
            originalBytes.Add(0x0d);
            originalBytes.read(1);

            Assert.Equal(new byte[] { 0x0b, 0x0c, 0x0d }, originalBytes._bytes);
            Assert.Equal(1UL, originalBytes.offset);
            Assert.Equal(new byte[] { 0x0a, 0x0b, 0x0c }, clonedBytes._bytes);
            Assert.Equal(0UL, clonedBytes.offset);

            // This is verbose test code (this tests C# language specs)
            Assert.Equal(new byte[] { 0x0a, 0x0b, 0x0c }, bytes);
            Assert.Equal(0UL, offset);
        }

        [Fact]
        public void ReadWithoutOffsetTest()
        {
            var sampleBytes = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10 };
            
            // Check length
            var bytes = new Bytes(sampleBytes, 0);
            Assert.Equal(17, bytes.length);

            // Read all
            bytes = new Bytes(sampleBytes, 0);
            var read = bytes.read();
            Assert.Equal(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10 }, read._bytes);
            Assert.Equal(0UL, read.offset);
            Assert.Equal(new byte[] {}, bytes._bytes);
            Assert.Equal(17UL, bytes.offset);

            // Read specific length
            bytes = new Bytes(sampleBytes, 0);
            read = bytes.read(5);
            Assert.Equal(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04 }, read._bytes);
            Assert.Equal(0UL, read.offset);
            Assert.Equal(new byte[] { 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10 }, bytes._bytes);
            Assert.Equal(5UL, bytes.offset);

            // Read beyond length
            bytes = new Bytes(sampleBytes, 0);
            Assert.Throws<Exception>(() =>
            {
                _ = bytes.read(100000);
            });;
        }

        [Fact]
        public void ReadWithOffsetTest()
        {
            var sampleBytes = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10 };
            
            // Check length
            var bytes = new Bytes(sampleBytes, 100);
            Assert.Equal(17, bytes.length);

            // Read all
            bytes = new Bytes(sampleBytes, 100);
            var read = bytes.read();
            Assert.Equal(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10 }, read._bytes);
            Assert.Equal(100UL, read.offset);
            Assert.Equal(new byte[] {}, bytes._bytes);
            Assert.Equal(117UL, bytes.offset);

            // Read specific length
            bytes = new Bytes(sampleBytes, 100);
            read = bytes.read(5);
            Assert.Equal(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04 }, read._bytes);
            Assert.Equal(100UL, read.offset);
            Assert.Equal(new byte[] { 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10 }, bytes._bytes);
            Assert.Equal(105UL, bytes.offset);

            // Read beyond length
            bytes = new Bytes(sampleBytes, 100);
            Assert.Throws<Exception>(() =>
            {
                _ = bytes.read(100000);
            });;
        }

        [Fact]
        public void AsciiEncodingTest()
        {
            var sampleAsciiBytes = new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x57, 0x6F, 0x72, 0x6C, 0x64 };
            var bytes = new Bytes(sampleAsciiBytes, 0);
            Assert.Equal("Hello World", bytes.ascii);

            // With not printable characters
            var sampleAsciiWithNonPrintableBytes = new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x00, 0x57, 0x6F, 0x72, 0x6C, 0x64 };
            bytes = new Bytes(sampleAsciiWithNonPrintableBytes, 0);
            Assert.Equal("Hello .World", bytes.ascii);

            // Check borderline for non-printable characters
            sampleAsciiWithNonPrintableBytes = new byte[] { 0x00, 0x1f, 0x20, 0x7e, 0x7f, 0x80, 0xff };
            bytes = new Bytes(sampleAsciiWithNonPrintableBytes, 0);
            Assert.Equal(".. ~...", bytes.ascii);
        }

        [Fact]
        public void UnicodeEncodingTest()
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
        public void HexPrintTest()
        {
            var input = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f };
            var bytes = new Bytes(input, 0);
            Assert.Equal("00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F", bytes.hex);
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
        public void SkipWithoutOffsetTest()
        {
            var sampleBytes = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10 };

            // Skip 10000
            var bytes = new Bytes(sampleBytes, 0);
            var res = bytes.skip(10000);
            Assert.Equal(new byte[] {}, res._bytes);
            Assert.Equal(10000UL, res.offset);

            // Skip 0
            bytes = new Bytes(sampleBytes, 0);
            res = bytes.skip(0);
            Assert.Equal(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10 }, res._bytes);
            Assert.Equal(0UL, res.offset);

            // Skip 13
            bytes = new Bytes(sampleBytes, 0);
            res = bytes.skip(13);
            Assert.Equal(new byte[] { 0x0d, 0x0e, 0x0f, 0x10 }, res._bytes);
            Assert.Equal(13UL, res.offset);
        }

        [Fact]
        public void SkipWithOffsetTest()
        {
            var sampleBytes = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10 };
            Bytes bytes;
            Bytes res;

            // Skip 10000 with offset
            bytes = new Bytes(sampleBytes, 100);
            res = bytes.skip(10000);
            Assert.Equal(new byte[] { }, res._bytes);
            Assert.Equal(10100UL, res.offset);

            // Skip 0 with offset
            bytes = new Bytes(sampleBytes, 100);
            res = bytes.skip(0);
            Assert.Equal(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10 }, res._bytes);
            Assert.Equal(100UL, res.offset);

            // Skip 13 with offset
            bytes = new Bytes(sampleBytes, 100);
            res = bytes.skip(13);
            Assert.Equal(new byte[] { 0x0d, 0x0e, 0x0f, 0x10 }, res._bytes);
            Assert.Equal(113UL, res.offset);

        }

        [Fact]
        public void TakeWithoutOffsetTest()
        {
            var sampleBytes = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10 };
            Bytes bytes;
            Bytes res;

            // Take 10000
            bytes = new Bytes(sampleBytes, 0);
            res = bytes.take(10000);
            Assert.Equal(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10 }, res._bytes);
            Assert.Equal(0UL, res.offset);

            // Take 0
            bytes = new Bytes(sampleBytes, 0);
            res = bytes.take(0);
            Assert.Equal(new byte[] { }, res._bytes);
            Assert.Equal(0UL, res.offset);

            // Take 4
            bytes = new Bytes(sampleBytes, 0);
            res = bytes.take(4);
            Assert.Equal(new byte[] { 0x00, 0x01, 0x02, 0x03 }, res._bytes);
            Assert.Equal(0UL, res.offset);
        }

        [Fact]
        public void TakeWithOffsetTest()
        {
            var sampleBytes = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10 };
            Bytes bytes;
            Bytes res;

            // Take 10000
            bytes = new Bytes(sampleBytes, 100);
            res = bytes.take(10000);
            Assert.Equal(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10 }, res._bytes);
            Assert.Equal(100UL, res.offset);

            // Take 0
            bytes = new Bytes(sampleBytes, 100);
            res = bytes.take(0);
            Assert.Equal(new byte[] { }, res._bytes);
            Assert.Equal(100UL, res.offset);

            // Take 4
            bytes = new Bytes(sampleBytes, 100);
            res = bytes.take(4);
            Assert.Equal(new byte[] { 0x00, 0x01, 0x02, 0x03 }, res._bytes);
            Assert.Equal(100UL, res.offset);
        }

        [Fact]
        public void ReverseTest()
        {
            var sampleBytes = new byte[] { 0x01, 0x02, 0x03, 0x04 };
            var bytes = new Bytes(sampleBytes, 0);
            var reversed = bytes.reverse();
            Assert.Equal(new byte[] { 0x04, 0x03, 0x02, 0x01 }, reversed._bytes);
            Assert.Equal(0UL, reversed.offset);

            // With offset
            bytes = new Bytes(sampleBytes, 100);
            reversed = bytes.reverse();
            Assert.Equal(new byte[] { 0x04, 0x03, 0x02, 0x01 }, reversed._bytes);
            Assert.Equal(100UL, reversed.offset);
        }

        [Fact]
        public void Le2BeTest()
        {
            var sampleBytes = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 };
            var bytes = new Bytes(sampleBytes, 0);
            var converted = bytes.le2be(2);
            Assert.Equal(new byte[] { 0x02, 0x01, 0x04, 0x03, 0x06, 0x05 }, converted._bytes);
            Assert.Equal(0UL, converted.offset);

            // Also test be2le here
            var converted2 = bytes.be2le(2);
            Assert.Equal(new byte[] { 0x02, 0x01, 0x04, 0x03, 0x06, 0x05 }, converted2._bytes);
            Assert.Equal(0UL, converted2.offset);

            // And with offset
            bytes = new Bytes(sampleBytes, 100);
            converted = bytes.le2be(2);
            Assert.Equal(new byte[] { 0x02, 0x01, 0x04, 0x03, 0x06, 0x05 }, converted._bytes);
            Assert.Equal(100UL, converted.offset);

            // Also test be2le here
            converted2 = bytes.be2le(2);
            Assert.Equal(new byte[] { 0x02, 0x01, 0x04, 0x03, 0x06, 0x05 }, converted2._bytes);
            Assert.Equal(100UL, converted2.offset);
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
        public void TestFindWithOneBytePattern()
        {
            var sampleByte = new byte[] { 0x01, 0x02, 0x03, 0x01, 0x02, 0x03 };
            var bytes = new Bytes(sampleByte, 0);
            var indexes = bytes.find([0x02]).ToArray();
            Assert.Equal(new ulong[] { 1, 4 }, indexes);

            // Index of (first match only)
            var index = bytes.indexOf([0x02]);
            Assert.Equal(1UL, index);

            // Find with ASCII
            var sampleAsciiByte = new byte[] { 0x61, 0x62, 0x63, 0x61, 0x62, 0x63 }; // "abcabc"
            var bytesAscii = new Bytes(sampleAsciiByte, 0);
            var indexesAscii = bytesAscii.find("b").ToArray();
            Assert.Equal(new ulong[] { 1, 4 }, indexesAscii);
        }

        [Fact]
        public void TestFindWithFewBytesPattern()
        {
            var sampleByte = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x01, 0x02, 0x03, 0x04, 0x01, 0x02, 0x03 };
            var bytes = new Bytes(sampleByte, 0);
            var indexes = bytes.find([0x02, 0x03, 0x04]).ToArray();
            Assert.Equal(new ulong[] { 1, 5 }, indexes);

            // Index of (first match only)
            var index = bytes.indexOf([0x02, 0x03, 0x04]);
            Assert.Equal(1UL, index);

            // Find with ASCII
            var sampleAsciiByte = new byte[] { 0x61, 0x62, 0x63, 0x64, 0x61, 0x62, 0x63, 0x64 }; // "abcdabcd"
            var bytesAscii = new Bytes(sampleAsciiByte, 0);
            var indexesAscii = bytesAscii.find("bc").ToArray();
            Assert.Equal(new ulong[] { 1, 5 }, indexesAscii);
        }

        // ToDo: Add Brotli, Deflate, GZip, and ZLib test
        // ToDo: Add Bytes to number conversion test

        [Fact]
        public void TestBcd()
        {
            byte[] sampleBytes;
            Bytes bytes;

            // Test BCD
            sampleBytes = new byte[] { 0x12, 0x34, 0x56 };
            bytes = new Bytes(sampleBytes, 0);
            Assert.Equal(563412UL, bytes.bcdLe());
            Assert.Equal(123456UL, bytes.bcdBe());

            // Test BCD with A+ tens
            sampleBytes = new byte[] { 0x12, 0xA4, 0x56 };
            bytes = new Bytes(sampleBytes, 0);
            Assert.Throws<ArgumentException>(() => bytes.bcdLe());
            Assert.Throws<ArgumentException>(() => bytes.bcdBe());

            // Test BCD with A+ ones
            sampleBytes = new byte[] { 0x12, 0x3A, 0x56 };
            bytes = new Bytes(sampleBytes, 0);
            Assert.Throws<ArgumentException>(() => bytes.bcdLe());
            Assert.Throws<ArgumentException>(() => bytes.bcdBe());
        }

        // ToDo: Add Hashing test

        [Fact]
        public void TestXorSameSize()
        {
            byte[] encrypted;
            Bytes encryptedBytes;
            byte[] key;
            Bytes keyBytes;
            Bytes decrypted;

            byte[] expected = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x61, 0x41 };

            encrypted = [ 0x01, 0x02, 0x03, 0x00, 0x00, 0x00, 0x41, 0x61 ];
            key = [0x01, 0x02, 0x03, 0x00, 0x00, 0x00, 0x20, 0x20 ];
            encryptedBytes = new Bytes(encrypted);
            keyBytes = new Bytes(key);

            // Test XOR with same size/both Bytes
            decrypted = encryptedBytes ^ keyBytes;
            Assert.Equal(expected, decrypted._bytes);

            // Test XOR with same size/Bytes and byte[]
            decrypted = encryptedBytes ^ key;
            Assert.Equal(expected, decrypted._bytes);

            // Test XOR with same size/byte[] and Bytes
            decrypted = encrypted ^ keyBytes;
            Assert.Equal(expected, decrypted._bytes);
        }

        [Fact]
        public void TestXorWithRightSmall()
        {
            byte[] encrypted;
            Bytes encryptedBytes;
            byte[] key;
            Bytes keyBytes;
            Bytes decrypted;

            byte[] expected = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x61, 0x41, 0xaa, 0x00 };

            encrypted = [ 0x01, 0x02, 0x03, 0x00, 0x00, 0x00, 0x41, 0x61, 0xaa, 0x00 ];
            key = [0x01, 0x02, 0x03, 0x00, 0x00, 0x00, 0x20, 0x20 ];
            encryptedBytes = new Bytes(encrypted);
            keyBytes = new Bytes(key);

            // Test XOR with both Bytes
            decrypted = encryptedBytes ^ keyBytes;
            Assert.Equal(expected, decrypted._bytes);

            // Test XOR with Bytes and byte[]
            decrypted = encryptedBytes ^ key;
            Assert.Equal(expected, decrypted._bytes);

            // Test XOR with byte[] and Bytes
            decrypted = encrypted ^ keyBytes;
            Assert.Equal(expected, decrypted._bytes);
        }

        [Fact]
        public void TestXorWithLeftSmall()
        {
            byte[] encrypted;
            Bytes encryptedBytes;
            byte[] key;
            Bytes keyBytes;
            Bytes decrypted;

            byte[] expected = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x61, 0x41 };

            encrypted = [ 0x01, 0x02, 0x03, 0x00, 0x00, 0x00, 0x41, 0x61 ];
            key = [0x01, 0x02, 0x03, 0x00, 0x00, 0x00, 0x20, 0x20, 0x20, 0x00, 0x20 ];
            encryptedBytes = new Bytes(encrypted);
            keyBytes = new Bytes(key);

            // Test XOR with both Bytes
            decrypted = encryptedBytes ^ keyBytes;
            Assert.Equal(expected, decrypted._bytes);

            // Test XOR with Bytes and byte[]
            decrypted = encryptedBytes ^ key;
            Assert.Equal(expected, decrypted._bytes);

            // Test XOR with byte[] and Bytes
            decrypted = encrypted ^ keyBytes;
            Assert.Equal(expected, decrypted._bytes);
        }

        [Fact]
        public void TestIsEqual()
        {
            Bytes testA = new Bytes([0x0a, 0x0b, 0x0c]);
            Bytes testB = new Bytes([0x0a, 0x0b, 0x0c], 100);
            Bytes testC = new Bytes([0x0d, 0x0e, 0x0f], 100);

            // Compare with same content
            Assert.True(testA == testB);
            Assert.True(testA == testB._bytes); // This is byte[] and byte[] comparison
            Assert.True(testA._bytes == testB);

            // Compare with same content but not
            Assert.False(testA != testB);
            Assert.False(testA != testB._bytes); // This is byte[] and byte[] comparison
            Assert.False(testA._bytes != testB);

            // Compare with different content
            Assert.False(testA == testC);
            Assert.False(testA == testC._bytes); // This is byte[] and byte[] comparison
            Assert.False(testA._bytes == testC);

            // Compare with different content but not
            Assert.True(testA != testC);
            Assert.True(testA != testC._bytes); // This is byte[] and byte[] comparison
            Assert.True(testA._bytes != testC);
        }

        [Fact]
        public void TestNullCompare()
        {
            Bytes testA = new Bytes([0x0a, 0x0b, 0x0c]);
            Bytes testB = null;

            // Compare with null on right
            Assert.False(testA == null);
            Assert.True(testA != null);

            // Compare with null on left
            Assert.False(null == testA);
            Assert.True(null != testA);

            // Null check on left variable
            Assert.True(testB == null);
            Assert.False(testB != null);

            // Null check on right variable
            Assert.True(null == testB);
            Assert.False(null != testB);

            // Compare with null-ed Bytes on right
            Assert.False(testA == testB);
            Assert.True(testA != testB);

            // Compare with null-ed Bytes on left
            Assert.False(testB == testA);
            Assert.True(testB != testA);
        }
    }
}
