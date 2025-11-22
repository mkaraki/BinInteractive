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
    }
}
