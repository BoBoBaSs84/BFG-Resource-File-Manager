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
	private const uint IMAGE_FILE_MAGIC = ('B' << 0) | ('I' << 8) | ('M' << 16) | (10 << 24);

	/// <summary>
	/// The timestamp of the bimage file.
	/// </summary>
	public DateTime TimeStamp { get; } = timeStamp;

	/// <summary>
	/// The "Magic" of the bimage file.
	/// </summary>
	public static uint Magic => IMAGE_FILE_MAGIC;

	/// <summary>
	/// The texture type of the bimage file.
	/// </summary>
	public TextureType TextureType { get; } = textureType;

	/// <summary>
	/// The texture format of the bimage file.
	/// </summary>
	public TextureFormat TextureFormat { get; } = textureFormat;

	/// <summary>
	/// The color format of the bimage file.
	/// </summary>
	public ColorFormat ColorFormat { get; } = colorFormat;

	/// <summary>
	/// The width of the bimage file.
	/// </summary>
	public uint Width { get; } = width;

	/// <summary>
	/// The height of the bimage file.
	/// </summary>
	public uint Height { get; } = height;

	/// <summary>
	/// The levels of the bimage file.
	/// </summary>
	public uint Levels { get; } = (uint)images.Length;

	/// <summary>
	/// The images of the bimage file.
	/// </summary>
	public Bimage[] Images { get; } = images;
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
	public uint Level { get; } = level;

	/// <summary>
	/// The destination of the image.
	/// </summary>
	public uint DestZ { get; } = destZ;

	/// <summary>
	/// The width of the image.
	/// </summary>
	public uint Width { get; } = width;

	/// <summary>
	/// The height of the image.
	/// </summary>
	public uint Height { get; } = height;

	/// <summary>
	/// The size of the image.
	/// </summary>
	public uint Size { get; } = size;

	/// <summary>
	/// The date of the image.
	/// </summary>
	public byte[] Data { get; } = data;
}
