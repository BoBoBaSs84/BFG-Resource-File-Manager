using ResourceFileEditor.Utils;

namespace ResourceFileEditorTests.Utils;

[TestClass]
public sealed class PathParserTests
{
	[DataTestMethod]
	[DataRow("root/child1/child2", 2)]
	[DataRow("root/child1/child2/child3", 3)]
	public void ParsePathTest(string value, int expected)
	{
		TreeNode node = PathParser.ParsePath(value);
		Assert.AreEqual(expected, node.GetNodeCount(true));
	}

	[DataTestMethod]
	[DataRow("root/child1/child2", "child1/child2/")]
	public void NodetoPathTest(string value, string expected)
	{
		TreeNode node = PathParser.ParsePath(value);
		string result = PathParser.NodeToPath(node.FirstNode.FirstNode);

		Assert.AreEqual(expected, result);
	}
}