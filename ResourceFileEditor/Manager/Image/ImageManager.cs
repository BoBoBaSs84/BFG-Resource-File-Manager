using System;
using System.Collections.Generic;
using System.IO;

namespace ResourceFileEditor.Manager.Image;

internal class ImageManager
{
	private const uint IMAGE_FILE_MAGIC = ('B' << 0) | ('I' << 8) | ('M' << 16) | (10 << 24);

	public static Stream LoadImage(Stream file)
	{
		Stream img = null;

		int index = 0;
		_ = DateTime.FromBinary((long)FileManager.FileManager.ReadUint64Swapped(file, index));
		index += 8;
		uint magic = FileManager.FileManager.ReadUint32Swapped(file, index);
		index += 4;
		if (magic == IMAGE_FILE_MAGIC)
		{
			uint texType = FileManager.FileManager.ReadUint32Swapped(file, index);
			index += 4;
			uint format = FileManager.FileManager.ReadUint32Swapped(file, index);
			index += 4;
			uint colorFormat = FileManager.FileManager.ReadUint32Swapped(file, index);
			index += 4;
			_ = FileManager.FileManager.ReadUint32Swapped(file, index);
			index += 4;
			_ = FileManager.FileManager.ReadUint32Swapped(file, index);
			index += 4;
			uint numLevels = FileManager.FileManager.ReadUint32Swapped(file, index);
			index += 4;
			if (texType == (uint)TextureType.TT_CUBIC)
			{
				numLevels *= 6;
			}

			List<ImageData> images = [];
			for (int i = 0; i < numLevels; i++)
			{
				images.Add(new ImageData());
				images[i].level = FileManager.FileManager.ReadUint32Swapped(file, index);
				index += 4;
				images[i].destZ = FileManager.FileManager.ReadUint32Swapped(file, index);
				index += 4;
				images[i].width = FileManager.FileManager.ReadUint32Swapped(file, index);
				index += 4;
				images[i].height = FileManager.FileManager.ReadUint32Swapped(file, index);
				index += 4;
				images[i].dataSize = FileManager.FileManager.ReadUint32Swapped(file, index);
				index += 4;
				images[i].data = FileManager.FileManager.ReadByteArray(file, index, (int)images[i].dataSize);
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
}
