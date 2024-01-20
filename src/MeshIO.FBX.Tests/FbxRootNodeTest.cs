using System;
using Xunit;

namespace MeshIO.FBX.Tests
{
	public class FbxRootNodeTest
	{
		public static readonly TheoryData<FbxVersion> Versions = FbxTestCasesData.Versions;

		[Theory]
		[MemberData(nameof(FbxTestCasesData.Versions))]
		public void CreateFromEmptySceneTest(FbxVersion version)
		{
		}
	}
}
