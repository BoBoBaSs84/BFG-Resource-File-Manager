using ResourceFileEditor.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using FM = ResourceFileEditor.FileManager.FileManager;

namespace ResourceFileEditor.Manager.Image;

public class ImageManager
{
	private const uint IMAGE_FILE_MAGIC = ('B' << 0) | ('I' << 8) | ('M' << 16) | (10 << 24);

	public static Stream LoadImage(Stream file)
	{
		Stream img = null;

		int index = 0;
		_ = DateTime.FromBinary((long)FM.ReadUint64Swapped(file, index));
		index += 8;
		uint magic = FM.ReadUint32Swapped(file, index);
		index += 4;
		if (magic == IMAGE_FILE_MAGIC)
		{
			uint texType = FM.ReadUint32Swapped(file, index);
			index += 4;
			uint format = FM.ReadUint32Swapped(file, index);
			index += 4;
			uint colorFormat = FM.ReadUint32Swapped(file, index);
			index += 4;
			_ = FM.ReadUint32Swapped(file, index);
			index += 4;
			_ = FM.ReadUint32Swapped(file, index);
			index += 4;
			uint numLevels = FM.ReadUint32Swapped(file, index);
			index += 4;
			if (texType == (uint)TextureType.TT_CUBIC)
			{
				numLevels *= 6;
			}

			List<ImageData> images = [];
			for (int i = 0; i < numLevels; i++)
			{
				images.Add(new ImageData());
				images[i].level = FM.ReadUint32Swapped(file, index);
				index += 4;
				images[i].destZ = FM.ReadUint32Swapped(file, index);
				index += 4;
				images[i].width = FM.ReadUint32Swapped(file, index);
				index += 4;
				images[i].height = FM.ReadUint32Swapped(file, index);
				index += 4;
				images[i].dataSize = FM.ReadUint32Swapped(file, index);
				index += 4;
				images[i].data = FM.ReadByteArray(file, index, (int)images[i].dataSize);
				index += (int)images[i].dataSize;
			}
			//TODO Multilevel Textures
			img = CovertToBitmap(images[0], texType, format, colorFormat);
		}

		return img;
	}

	private static Stream CovertToBitmap(ImageData image, uint texType, uint format, uint colorFormat)
	{
		byte[] data = image.data;
		Stream imageStream = new MemoryStream();
		if (format is not ((uint)TextureFormat.FMT_RGBA8) and not ((uint)TextureFormat.FMT_RGB565))
		{
			BCnEncoder.Decoder.BcDecoder decoder = new();
			BCnEncoder.Shared.CompressionFormat compressionFormat = BCnEncoder.Shared.CompressionFormat.BC1WithAlpha;
			switch ((TextureFormat)format)
			{
				case TextureFormat.FMT_DXT5:
					compressionFormat = BCnEncoder.Shared.CompressionFormat.BC3;
					break;
				case TextureFormat.FMT_DXT1:
					compressionFormat = BCnEncoder.Shared.CompressionFormat.BC1;
					break;
				case TextureFormat.FMT_ALPHA:
				case TextureFormat.FMT_LUM8:
					compressionFormat = BCnEncoder.Shared.CompressionFormat.BC4;
					break;
			}
			data = decoder.DecodeRawData(image.data, (int)image.width, (int)image.height, compressionFormat);
		}
		else if (format == (uint)TextureFormat.FMT_RGB565)
		{
			Stream parsedImageBuffer = new MemoryStream();
			ushort red_mask = 0xF800;
			ushort green_mask = 0x7E0;
			ushort blue_mask = 0x1F;
			Stream imageBuffer = new MemoryStream(data);
			int index = 0;
			while (index < imageBuffer.Length)
			{
				byte[] pixelBuffer = new byte[2];
				_ = imageBuffer.Read(pixelBuffer, 0, 2);
				index += 2;
				ushort pixel = BitConverter.ToUInt16(pixelBuffer, 0);
				byte red = (byte)((pixel & red_mask) >> 11);
				byte green = (byte)((pixel & green_mask) >> 5);
				byte blue = (byte)(pixel & blue_mask);
				//This might not be right
				red = (byte)(red << 3);
				green = (byte)(green << 2);
				blue = (byte)(blue << 3);
				byte[] parsedPixel = new byte[4];
				parsedPixel[0] = red;
				parsedPixel[1] = green;
				parsedPixel[2] = blue;
				parsedPixel[3] = 255;
				parsedImageBuffer.Write(parsedPixel, 0, 4);
			}
			parsedImageBuffer.Position = 0;
			data = new byte[parsedImageBuffer.Length];
			_ = parsedImageBuffer.Read(data, 0, (int)parsedImageBuffer.Length);
		}

		StbImageWriteSharp.ImageWriter imageWriter = new();
		imageWriter.WriteTga(data, (int)image.width, (int)image.height, StbImageWriteSharp.ColorComponents.RedGreenBlueAlpha, imageStream);
		return imageStream;
	}

	/// <summary>
	/// Loads a <see cref="BimageFile"/> from the provided <see cref="Stream"/>.
	/// </summary>
	/// <param name="stream">The bimage file stream.</param>
	/// <returns>The created <see cref="BimageFile"/>.</returns>
	/// <exception cref="FileFormatException">If the provided image is not a bimage format.</exception>
	public static BimageFile LoadBimage(Stream stream)
	{
		int index = default;
		DateTime timeStamp = DateTime.FromBinary((long)FM.ReadUint64Swapped(stream, index));
		index += 8;
		uint magic = FM.ReadUint32Swapped(stream, index);
		index += 4;

		if (magic != IMAGE_FILE_MAGIC)
			throw new FileFormatException("The provided image is not a bimage format.");

		TextureType textureType = (TextureType)FM.ReadUint32Swapped(stream, index);
		index += 4;
		TextureFormat textureFormat = (TextureFormat)FM.ReadUint32Swapped(stream, index);
		index += 4;
		ColorFormat colorFormat = (ColorFormat)FM.ReadUint32Swapped(stream, index);
		index += 4;
		uint width = FM.ReadUint32Swapped(stream, index);
		index += 4;
		uint height = FM.ReadUint32Swapped(stream, index);
		index += 4;
		uint levels = FM.ReadUint32Swapped(stream, index);
		index += 4;

		if (textureType == TextureType.TT_CUBIC)
			levels *= 6;

		List<Bimage> bimages = [];
		for (int i = 0; i < levels; i++)
		{
			uint iLevel = FM.ReadUint32Swapped(stream, index);
			index += 4;
			uint iDestZ = FM.ReadUint32Swapped(stream, index);
			index += 4;
			uint iWidth = FM.ReadUint32Swapped(stream, index);
			index += 4;
			uint iHeight = FM.ReadUint32Swapped(stream, index);
			index += 4;
			uint iSize = FM.ReadUint32Swapped(stream, index);
			index += 4;
			byte[] iData = FM.ReadByteArray(stream, index, (int)iSize);
			index += (int)iSize;

			Bimage bimage = new(iLevel, iDestZ, iWidth, iHeight, iSize, iData);
			bimages.Add(bimage);
		}

		return new(timeStamp, textureType, textureFormat, colorFormat, width, height, [.. bimages]);
	}

	/// <summary>
	/// Saves a provided <see cref="BimageFile"/> into a <see cref="Stream"/>.
	/// </summary>
	/// <param name="bimage">The <see cref="BimageFile"/> to save.</param>
	/// <returns>A <see cref="Stream"/> that is <u><b>not</b></u> disposed.</returns>
	public static Stream SaveBimage(BimageFile bimage)
	{
		int index = default;
		MemoryStream stream = new();

		FM.WriteUint64Swapped(stream, index, (ulong)bimage.TimeStamp.Ticks);
		index += 8;
		FM.WriteUint32Swapped(stream, index, BimageFile.Magic);
		index += 4;
		FM.WriteUint32Swapped(stream, index, (uint)bimage.TextureType);
		index += 4;
		FM.WriteUint32Swapped(stream, index, (uint)bimage.TextureFormat);
		index += 4;
		FM.WriteUint32Swapped(stream, index, (uint)bimage.ColorFormat);
		index += 4;
		FM.WriteUint32Swapped(stream, index, bimage.Width);
		index += 4;
		FM.WriteUint32Swapped(stream, index, bimage.Height);
		index += 4;
		FM.WriteUint32Swapped(stream, index, bimage.Levels);
		index += 4;

		foreach (Bimage image in bimage.Images)
		{
			FM.WriteUint32Swapped(stream, index, image.Level);
			index += 4;
			FM.WriteUint32Swapped(stream, index, image.DestZ);
			index += 4;
			FM.WriteUint32Swapped(stream, index, image.Width);
			index += 4;
			FM.WriteUint32Swapped(stream, index, image.Height);
			index += 4;
			FM.WriteUint32Swapped(stream, index, image.Size);
			index += 4;
			FM.WriteByteArray(stream, index, image.Data);
			index += (int)image.Size;
		}

		return stream;
	}
}
