using MeshIO.Entities.Geometries;
using Xunit;

namespace MeshIO.FBX.Tests
{
	public static  class TestCasesData
	{
		public static readonly TheoryData<FbxVersion> Versions = new TheoryData<FbxVersion>();

		static TestCasesData()
		{
			//Only compatible version
			Versions.Add(FbxVersion.v7000);
			Versions.Add(FbxVersion.v7100);
			Versions.Add(FbxVersion.v7200);
			Versions.Add(FbxVersion.v7300);
			Versions.Add(FbxVersion.v7400);
			Versions.Add(FbxVersion.v7500);
			Versions.Add(FbxVersion.v7600);
			Versions.Add(FbxVersion.v7700);
		}

		public static Scene CreateBoxScene()
		{
			Scene scene = new Scene();
			scene.RootNode.Nodes.Add(new Mesh("Box"));

			return scene;
		}
	}
}
