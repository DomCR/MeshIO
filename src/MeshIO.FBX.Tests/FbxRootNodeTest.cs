using System;
using Xunit;

namespace MeshIO.FBX.Tests
{
	public class FbxRootNodeTest
	{
		public static readonly TheoryData<FbxVersion> Versions = TestCasesData.Versions;

		[Theory]
		[MemberData(nameof(TestCasesData.Versions))]
		public void CreateFromEmptySceneTest(FbxVersion version)
		{
			FbxRootNode root = FbxRootNode.CreateFromScene(new Scene(), version);
		}
	}
}
