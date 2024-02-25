using ResourceFileEditor.Utils;

namespace ResourceFileEditorTests.Utils;

[TestClass]
public sealed class ByteSwapTests
{
	[DataTestMethod]
	[DataRow(new byte[] { 00, 255 }, new byte[] { 255, 00 })]
	[DataRow(new byte[] { 32, 64, 96, 128 }, new byte[] { 128, 96, 64, 32 })]
	public void SwapBytesTest(byte[] values, byte[] expected)
	{
		ByteSwap.SwapBytes(values);

		Assert.IsTrue(values.SequenceEqual(expected));
	}
}