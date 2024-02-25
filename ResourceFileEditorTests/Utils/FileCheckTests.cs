using ResourceFileEditor.Utils;
using FileTypes = ResourceFileEditor.Utils.FileCheck.FileTypes;

namespace ResourceFileEditorTests.Utils;

[TestClass]
public sealed class FileCheckTests
{
	[DataTestMethod]
	[DataRow("file.jpg", true)]
	[DataRow("folder", false)]
	public void IsFileTest(string value, bool expected)
		=> Assert.AreEqual(expected, FileCheck.IsFile(value));

	[DataTestMethod]
	[DataRow("file.idwav", true)]
	[DataRow("file.bimage", true)]
	[DataRow("file.dev", false)]
	public void IsExportableToStandardTest(string value, bool expected)
		=> Assert.AreEqual(expected, FileCheck.IsExportableToStandard(value));

	[DataTestMethod]
	[DataRow("file.tga", FileTypes.IMAGE)]
	[DataRow("file.jpg", FileTypes.IMAGE)]
	[DataRow("file.png", FileTypes.IMAGE)]
	[DataRow("file.bimage", FileTypes.IMAGE)]
	[DataRow("file.wav", FileTypes.AUDIO)]
	[DataRow("file.idwav", FileTypes.AUDIO)]
	[DataRow("file.txt", FileTypes.UNKNOWN)]
	public void GetFileTypeTest(string value, FileTypes expected)
		=> Assert.AreEqual(expected, FileCheck.GetFileType(null, value));

	[TestMethod]
	public void GetPathSeparatorTest()
	{
		string separator = Path.DirectorySeparatorChar.ToString();
		Assert.AreEqual(separator, FileCheck.GetPathSeparator());
	}
}