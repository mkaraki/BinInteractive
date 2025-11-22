using BinPlayground.Types;

namespace BinPlayground.Tests
{
    public class BytesBitmap1DTest
    {
        [Fact]
        public void ColorGetTest()
        {
            var sampleBytes = new byte[] { 0x14, 0x53, 0x42, 0x00 };
            var bytes = new Bytes(sampleBytes);

            var bitmap1d = bytes.bitmap1d;
            Assert.Equal(4UL, bitmap1d.Length);
            Assert.Equal(new byte[] { 0x14, 0x14, 0x14 }, bitmap1d.GetColor(0));
            Assert.Equal(new byte[] { 0x53, 0x53, 0x53 }, bitmap1d.GetColor(1));
            Assert.Equal(new byte[] { 0x42, 0x42, 0x42 }, bitmap1d.GetColor(2));
            Assert.Equal(new byte[] { 0x00, 0x00, 0x00 }, bitmap1d.GetColor(3));
        }
    }
}
