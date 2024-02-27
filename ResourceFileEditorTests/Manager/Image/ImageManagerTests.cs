﻿using ResourceFileEditor.Manager.Image;

namespace ResourceFileEditorTests.Manager.Image;

[TestClass]
public sealed class ImageManagerTests
{
	private static readonly string TestDirectory = Path.Combine(Environment.CurrentDirectory, "Resources");

	[TestMethod]
	public void LoadBimageTest()
	{
		string filePath = Path.Combine(TestDirectory, "loadingicon3#__0200.bimage");
		using FileStream fileStream = File.OpenRead(filePath);

		BimageFile bimageFile = ImageManager.LoadBimage(fileStream);

		Assert.IsNotNull(bimageFile);
		Assert.AreNotEqual(DateTime.MinValue, bimageFile.TimeStamp);
		Assert.AreEqual(TextureType.TT_2D, bimageFile.TextureType);
		Assert.AreEqual(TextureFormat.FMT_DXT5, bimageFile.TextureFormat);
		Assert.AreEqual(ColorFormat.CFM_DEFAULT, bimageFile.ColorFormat);
		Assert.AreEqual(256u, bimageFile.Width);
		Assert.AreEqual(256u, bimageFile.Height);
		Assert.AreEqual(7u, bimageFile.Levels);
		Assert.AreEqual((int)bimageFile.Levels, bimageFile.Images.Length);
	}

	[TestMethod]
	public void SaveBimageTest()
	{
		string filePath = Path.Combine(TestDirectory, "loadingicon3#__0200.bimage");
		using FileStream fileStream = File.OpenRead(filePath);
		BimageFile bimageFile = ImageManager.LoadBimage(fileStream);

		Stream stream = ImageManager.SaveBimage(bimageFile);

		Assert.AreEqual(fileStream.Length, stream.Length);
	}
}