using ResourceFileEditor.Utils;

namespace ResourceFileEditorTests.Utils;

[TestClass]
public sealed class NodeUtilsTests
{
	[TestMethod]
	public void FindByNameTest()
	{
		TreeNode root = new("root");
		_ = root.Nodes.Add("child1");

		Assert.IsNotNull(NodeUtils.FindByName(root.Nodes, "child1"));
		Assert.IsNull(NodeUtils.FindByName(root.Nodes, "child2"));
	}
}