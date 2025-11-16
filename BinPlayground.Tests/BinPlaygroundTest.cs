using BinPlayground.Types;

namespace BinPlayground.Tests
{
    public class BinPlaygroundTest
    {
        [Fact]
        public void Length()
        {
            // Empty stream
            var sampleBytes = new byte[] { };
            using (var ms = new MemoryStream(sampleBytes))
            {
                var playground = new BinPlayground(ms, new InteractiveConfig());
                Assert.Equal((long)0, playground.length);
            }

            // len=17 stream
            sampleBytes = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10 };
            using (var ms = new MemoryStream(sampleBytes))
            {
                var playground = new BinPlayground(ms, new InteractiveConfig());
                Assert.Equal((long)17, playground.length);
            }

        }

        [Fact]
        public void ReadBytes_FromStream()
        {
            var sampleByte = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10 };
            using var ms = new MemoryStream(sampleByte);

            var playground = new BinPlayground(ms, new InteractiveConfig());

            // Read first 4 byte
            var bytes = playground.read(4);
            Assert.Equal(new byte[] { 0x00, 0x01, 0x02, 0x03 }, bytes._bytes);

            // Read next 4 byte
            bytes = playground.read(4);
            Assert.Equal(new byte[] { 0x04, 0x05, 0x06, 0x07 }, bytes._bytes);

            // Read remaining byte
            bytes = playground.read();
            Assert.Equal(new byte[] { 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10 }, bytes._bytes);

            // Read from pos=2, len=4
            playground.pos = 2;
            bytes = playground.read(4);
            Assert.Equal(new byte[] { 0x02, 0x03, 0x04, 0x05 }, bytes._bytes);

            // Seek (skip) 2byte then read 4byte
            playground.seek(2);
            bytes = playground.read(4);
            Assert.Equal(new byte[] { 0x08, 0x09, 0x0a, 0x0b }, bytes._bytes);

            // Seek outside
            playground.pos = 1000000;
            bytes = playground.read(4);
            Assert.Equal(new byte[] { }, bytes._bytes);
        }

        [Fact]
        public void ReadBytes_AbsolutePositionCheck()
        {
            var sampleByte = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10 };
            using var ms = new MemoryStream(sampleByte);

            var playground = new BinPlayground(ms, new InteractiveConfig());

            // Read pos=0, len=4, abs=true
            playground.pos = 0;
            var bytes = playground.read(4, true);
            Assert.Equal(new byte[] { 0x00, 0x01, 0x02, 0x03 }, bytes._bytes);
            Assert.Equal((ulong)0, bytes.offset);

            // Read pos=2, len=4, abs=true
            playground.pos = 2;
            bytes = playground.read(4, true);
            Assert.Equal(new byte[] { 0x02, 0x03, 0x04, 0x05 }, bytes._bytes);
            Assert.Equal((ulong)2, bytes.offset);

            // Read pos=13, len=ALL, abs=true
            playground.pos = 13;
            bytes = playground.read(true);
            Assert.Equal(new byte[] { 0x0d, 0x0e, 0x0f, 0x10 }, bytes._bytes);
            Assert.Equal((ulong)13, bytes.offset);

            // Read pos=0, len=4, abs=false
            playground.pos = 0;
            bytes = playground.read(4, false);
            Assert.Equal(new byte[] { 0x00, 0x01, 0x02, 0x03 }, bytes._bytes);
            Assert.Equal((ulong)0, bytes.offset);

            // Read pos=2, len=4, abs=false
            playground.pos = 2;
            bytes = playground.read(4, false);
            Assert.Equal(new byte[] { 0x02, 0x03, 0x04, 0x05 }, bytes._bytes);
            Assert.Equal((ulong)0, bytes.offset);

            // Read pos=13, len=ALL, abs=false
            playground.pos = 13;
            bytes = playground.read(false);
            Assert.Equal(new byte[] { 0x0d, 0x0e, 0x0f, 0x10 }, bytes._bytes);
            Assert.Equal((ulong)0, bytes.offset);
        }
    }
}
