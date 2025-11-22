using BinPlayground.Types;

namespace BinPlayground.Tests
{
    public class BytesBitmapTest
    {
        [Fact]
        public void ReadPixelTest()
        {
            var sampleBytes = new byte[] { 0x94, 0x24, 0x52, 0x53, 0x62, 0x53, 0x00, 0x00, 0x00 };
            var bytes = new Bytes(sampleBytes);
            var bitmap = bytes.bitmap;

            Assert.Equal(3UL, bitmap.Length);
            Assert.Equal(new byte[] { 0x94, 0x24, 0x52 }, bitmap.GetColor(0));
            Assert.Equal(new byte[] { 0x53, 0x62, 0x53 }, bitmap.GetColor(1));
            Assert.Equal(new byte[] { 0x00, 0x00, 0x00 }, bitmap.GetColor(2));

            bitmap = new BytesBitmap(bytes);

            Assert.Equal(3UL, bitmap.Length);
            Assert.Equal(new byte[] { 0x94, 0x24, 0x52 }, bitmap.GetColor(0));
            Assert.Equal(new byte[] { 0x53, 0x62, 0x53 }, bitmap.GetColor(1));
            Assert.Equal(new byte[] { 0x00, 0x00, 0x00 }, bitmap.GetColor(2));
        }

        [Fact]
        public void ReadMissingColorTest()
        {
            var sampleBytes = new byte[] { 0x35, 0x21 };
            var bitmap = new BytesBitmap(sampleBytes);

            Assert.Equal(1UL, bitmap.Length);
            Assert.Equal(new byte[] { 0x35, 0x21, 0x00 }, bitmap.GetColor(0));
        }

        [Fact]
        public void ReadOutboundPixelTest()
        {
            var sampleBytes = new byte[] { 0x12, 0x34, 0x56 };
            var bitmap = new BytesBitmap(sampleBytes);
            Assert.Equal(new byte[] { 0x00, 0x00, 0x00 }, bitmap.GetColor(1));
        }
    }
}
