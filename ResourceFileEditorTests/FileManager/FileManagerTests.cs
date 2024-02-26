using System.Text;

using FM = ResourceFileEditor.FileManager.FileManager;

namespace ResourceFileEditorTests.FileManager;

[TestClass]
public sealed class FileManagerTests
{
	[DataTestMethod]
	[DataRow("Doom", 28484)]
	public void ReadUint16Test(string value, int expected)
	{
		Stream stream = CreateTestStream(value);

		Assert.AreEqual(expected, FM.ReadUint16(stream, 0));
	}

	[DataTestMethod]
	[DataRow("Doom", 17519)]
	public void ReadUint16SwappedTest(string value, int expected)
	{
		Stream stream = CreateTestStream(value);

		Assert.AreEqual(expected, FM.ReadUint16Swapped(stream, 0));
	}

	[DataTestMethod]
	[DataRow("Doom", 1836019524u)]
	public void ReadUint32Test(string value, uint expected)
	{
		Stream stream = CreateTestStream(value);

		Assert.AreEqual(expected, FM.ReadUint32(stream, 0));
	}

	[DataTestMethod]
	[DataRow("Doom", 1148153709u)]
	public void ReadUint32SwappedTest(string value, uint expected)
	{
		Stream stream = CreateTestStream(value);

		Assert.AreEqual(expected, FM.ReadUint32Swapped(stream, 0));
	}

	[DataTestMethod]
	[DataRow("Doom", 1836019524)]
	public void ReadInt32Test(string value, int expected)
	{
		Stream stream = CreateTestStream(value);

		Assert.AreEqual(expected, FM.ReadInt32(stream, 0));
	}

	[DataTestMethod]
	[DataRow("Doom", 1148153709)]
	public void ReadInt32SwappedTest(string value, int expected)
	{
		Stream stream = CreateTestStream(value);

		Assert.AreEqual(expected, FM.ReadInt32Swapped(stream, 0));
	}

	[DataTestMethod]
	[DataRow("DoomDoom", 7885643812233506628ul)]
	public void ReadUint64Test(string value, ulong expected)
	{
		Stream stream = CreateTestStream(value);

		Assert.AreEqual(expected, FM.ReadUint64(stream, 0));
	}

	[DataTestMethod]
	[DataRow("DoomDoom", 4931282632084254573ul)]
	public void ReadUint64SwappedTest(string value, ulong expected)
	{
		Stream stream = CreateTestStream(value);

		Assert.AreEqual(expected, FM.ReadUint64Swapped(stream, 0));
	}

	[DataTestMethod]
	[DataRow("DoomDoom", new byte[] { 68, 111, 111, 109 })]
	public void ReadByteArrayTest(string value, byte[] expected)
	{
		Stream stream = CreateTestStream(value);

		byte[] result = FM.ReadByteArray(stream, 0, 4);
		
		Assert.IsTrue(expected.SequenceEqual(result));
	}

	private static MemoryStream CreateTestStream(string value)
		=> new(Encoding.UTF8.GetBytes(value));
}