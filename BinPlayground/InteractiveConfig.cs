namespace BinPlayground
{
    public class InteractiveConfig
    {
        public int BitmapWidth = 48;

        public int HexWidth = 16;

        public ColoredHex ColoredHex = ColoredHex.Normal;

        public bool OmitAsciiFromHex = false;

        public string OverrideHexViewEncoding = "ascii";
    }

    public enum ColoredHex
    {
        Normal,
        Ascii,
        Disabled,
    }
}
