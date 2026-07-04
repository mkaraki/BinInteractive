## Basic Usage

```
> BinInteractive.exe <file-to-play>
```

## Prompt

```
1::00000016> command you type
^  ~~~~~~~^
Cmd No.   |
Current Position (hex)
```

## Basic Commands

### [BinPlayground].seek

```cs
seek(0x30) // Seek 0x30 bytes forward
seek(-0x20) // Seek 0x20 bytes backward
seek(28) // Seek 28 bytes forward
```

### [BinPlayground].pos

```cs
pos = 0x30 // Go to offset 0x30
pos = 0 // Go to offset 0
```

### [BinPlayground].read, [Bytes].read

```cs
read() // Read remaining bytes. Returns a `Bytes` object.
read(0x20) // Read 0x20 bytes. Returns a `Bytes` object.

var b = read(0x100);
b.read(0x20) // Read 0x20 bytes from the `Bytes` object. Returns a new `Bytes` object.
b // Remain 0x80 bytes in the `Bytes` object.
```

### [Bytes].ascii, utf8, ... / [BinPlayground].ascii, utf8, and more

```cs
read().ascii // Display remaining bytes as an encoded string.
read().utf8 // Display remaining bytes as an encoded string.
read().utf16 // Display remaining bytes as an encoded string.
// read().wide // Same as utf16
read().utf32 // Display remaining bytes as an encoded string.

// Also works with just `ascii`, `utf8`, `utf16`, `utf32` commands.
ascii
utf8
utf16
utf32
```

### [Bytes].hex / [BinPlayground].hex

```cs
read().hex // Display remaining bytes as a hex dump.
hex // Same as above
```

### [Bytes].bitmap / [BinPlayground].bitmap

```cs
read().bitmap // Display remaining bytes as a bitmap image.
bitmap // Same as above
```

### [Bytes].bitmap1d // [BinPlayground].bitmap1d

This also shows a bitmap but uses 1 byte per pixel.

```cs
read().bitmap1d // Display remaining bytes as a bitmap image.
bitmap1d // Same as above
```

### [Bytes].magic / [BinPlayground].magic

This may not work on some platforms.

```cs
read().magic // Guess file type with libmagic (similar to the `file` command)
read().file // Same
magic // Same
```

### [Bytes].stencil(stencilName) / [BinPlayground].stencil(stencilName)

Stencil feature will try to decode the remaining bytes with a IStencil interface.
This is experimental and may not work well.

```cs
read().stencil("png") // Decode remaining bytes as a PNG image and show description of each chunk.
stencil("png") // Same as above
```

### [BinPlayground].decodeBrotli(), decodeDeflate(), and more

```cs
read().decodeBrotli() // Decode remaining bytes as Brotli compressed data.
read().decodeDeflate() // Decode remaining bytes as Deflate compressed data.
read().decodeGzip() // Decode remaining bytes as Gzip compressed data.
read().decodeZlib() // Decode remaining bytes as Zlib compressed data.
decodeBrotli() // Same as above
// also works with decodeDeflate(), decodeGzip(), decodeZlib(), etc.
```

### [Bytes].bcdBe() / [Bytes].bcdLe()

Supports up to 8 bytes (64 bits) of BCD data.

```cs
read().bcdBe() // Decode remaining bytes as big-endian BCD data.
read().bcdLe() // Decode remaining bytes as little-endian BCD data.
```

### [Bytes].md5(), [Bytes].sha1(), [Bytes].sha512(), and more

```cs
read().md5() // Calculate MD5 hash of remaining bytes.
read().sha1() // Calculate SHA-1 hash of remaining bytes.
read().sha256() // Calculate SHA-256 hash of remaining bytes.
read().sha384() // Calculate SHA-384 hash of remaining bytes.
read().sha512() // Calculate SHA-512 hash of remaining bytes.
read().sha3_256() // Calculate SHA3-256 hash of remaining bytes.
read().sha3_384() // Calculate SHA3-384 hash of remaining bytes.
read().sha3_512() // Calculate SHA3-512 hash of remaining bytes.
```

## Operators for Bytes

### [Bytes] ^ [Bytes], [Bytes] ^ byte[], byte[] ^ [Bytes]

```cs
read(0x20) ^ read(0x20) // XOR two Bytes objects.
read(2) ^ new Bytes { 0x00, 0x01, 0x02 } // XOR Bytes object with a Bytes object. Returns 2 bytes.
read(5) ^ new byte[] { 0x00, 0x01 } // XOR Bytes object with a byte array. Returns 5 bytes. XOR is performed with the first 2 bytes of the byte array, and the remaining 3 bytes are unchanged.
```

### [Bytes] == [Bytes], [Bytes] == byte[]

```cs
read(0x20) == read(0x20) // Compare two Bytes objects for equality.
read(3) == new Bytes { 0x00, 0x01, 0x02 } // Compare Bytes object with a Bytes object for equality.
read(3) == new byte[] { 0x00, 0x01, 0x02 } // Compare Bytes object with a byte array for equality.
```

## Configuration

See [InteractiveConfig] for more configuration options.

```cs
config.BitmapWidth = 48; // Set the width of bitmap images.
config.HexWidth = 16; // Set the number of bytes per line in hex view.
config.ColoredHex = BinInteractive.ColoredHex.Normal; // Set the color scheme for hex view. Options: Normal, Ascii, Disabled.
config.OmitAsciiFromHex = false; // Omit the ASCII column in hex view.
config.OverrideHexViewEncoding = "ascii"; // (Not recommended) Change encoding to use for the ASCII column in hex view. Options: ascii, utf8, utf16, wide, utf32.
```

[InteractiveConfig]: @ref BinPlayground.InteractiveConfig
[Bytes]: @ref BinPlayground.Types.Bytes
[BinPlayground]: @ref BinPlayground.BinPlayground