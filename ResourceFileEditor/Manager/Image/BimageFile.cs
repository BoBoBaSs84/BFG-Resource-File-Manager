using System;

namespace ResourceFileEditor.Manager.Image;

/// <summary>
/// The bimage file struct.
/// </summary>
/// <param name="timeStamp">The timestamp of the bimage file.</param>
/// <param name="textureType">The texture type of the bimage file.</param>
/// <param name="textureFormat">The texture format of the bimage file.</param>
/// <param name="colorFormat">The color format of the bimage file.</param>
/// <param name="width">The width of the bimage file.</param>
/// <param name="height">The height of the bimage file.</param>
/// <param name="images">The images of the bimage file.</param>
public readonly struct BimageFile(DateTime timeStamp, TextureType textureType, TextureFormat textureFormat, ColorFormat colorFormat, uint width, uint height, Bimage[] images)
{
	/// <summary>
	/// The version of the bimage file.
	/// </summary>
	public const int Version = 10;

	/// <summary>
	/// The "Magic" of the bimage file.
	/// </summary>
	public const uint Magic = ('B' << 0) | ('I' << 8) | ('M' << 16) | (Version << 24);

	/// <summary>
	/// The timestamp of the bimage file.
	/// </summary>
	public DateTime TimeStamp => timeStamp;

	/// <summary>
	/// The texture type of the bimage file.
	/// </summary>
	public TextureType TextureType => textureType;

	/// <summary>
	/// The texture format of the bimage file.
	/// </summary>
	public TextureFormat TextureFormat => textureFormat;

	/// <summary>
	/// The color format of the bimage file.
	/// </summary>
	public ColorFormat ColorFormat => colorFormat;

	/// <summary>
	/// The width of the bimage file.
	/// </summary>
	public uint Width => width;

	/// <summary>
	/// The height of the bimage file.
	/// </summary>
	public uint Height => height;

	/// <summary>
	/// The levels of the bimage file.
	/// </summary>
	public uint Levels => (uint)images.Length;

	/// <summary>
	/// The images of the bimage file.
	/// </summary>
	public Bimage[] Images => images;
}

/// <summary>
/// The bimage struct.
/// </summary>
/// <param name="level">The level of the image.</param>
/// <param name="destZ">The destination of the image.</param>
/// <param name="width">The width of the image.</param>
/// <param name="height">The height of the image.</param>
/// <param name="size">The size of the image.</param>
/// <param name="data">The date of the image.</param>
public readonly struct Bimage(uint level, uint destZ, uint width, uint height, uint size, byte[] data)
{
	/// <summary>
	/// The level of the image.
	/// </summary>
	public uint Level => level;

	/// <summary>
	/// The destination of the image.
	/// </summary>
	public uint DestZ => destZ;

	/// <summary>
	/// The width of the image.
	/// </summary>
	public uint Width => width;

	/// <summary>
	/// The height of the image.
	/// </summary>
	public uint Height => height;

	/// <summary>
	/// The size of the image.
	/// </summary>
	public uint Size => size;

	/// <summary>
	/// The date of the image.
	/// </summary>
	public byte[] Data => data;
}

/// <summary>
/// The bimage type enumerator.
/// </summary>
public enum BImageType : uint
{
	Disabled,
	TwoD,
	Cubic
}

/// <summary>
/// The bimage format enumerator.
/// </summary>
public enum BImageFormat : uint
{
	None,

	//------------------------
	// Standard color image formats
	//------------------------

	RGBA8,      // 32 bpp
	XRGB8,      // 32 bpp

	//------------------------
	// Alpha channel only
	//------------------------

	// Alpha ends up being the same as L8A8 in our current implementation, because straight 
	// alpha gives 0 for color, but we want 1.
	Alpha,

	//------------------------
	// Luminance replicates the value across RGB with a constant A of 255
	// Intensity replicates the value across RGBA
	//------------------------

	L8A8,     // 16 bpp
	Luminance8,   //  8 bpp
	Intensity8,   //  8 bpp

	//------------------------
	// Compressed texture formats
	//------------------------

	Dxt1,     // 4 bpp
	Dxt5,     // 8 bpp

	//------------------------
	// Depth buffer formats
	//------------------------

	Depth,      // 24 bpp

	//------------------------
	//
	//------------------------

	X16,      // 16 bpp
	Y16X16,     // 32 bpp
	RGB565      // 16 bpp
}

/// <summary>
/// The bimage color format enumerator.
/// </summary>
public enum BImageColorFormat : uint
{
	/// <summary>
	/// RGBA.
	/// </summary>
	Default,

	/// <summary>
	/// XY format and use the fast DXT5 compressor.
	/// </summary>
	NormalDxt5,

	/// <summary>
	/// Convert RGBA to CoCg_Y format.
	/// </summary>
	YCoCgDxt5,

	/// <summary>
	/// Copy the alpha channel to green.
	/// </summary>
	GreenAlpha
}