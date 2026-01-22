using MeshIO.Entities.Geometries;
using MeshIO.Entities.Primitives;
using MeshIO.Formats;
using MeshIO.Formats.Fbx;
using MeshIO.Tests.Common;
using MeshIO.Tests.TestModels;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.Tests.Formats.Fbx;

public class FbxWriterTests : IOTestsBase
{
	public static TheoryData<FbxFileModel> TestCases { get; } = new();

	public static readonly TheoryData<FbxVersion> Versions = FbxTestCasesData.Versions;

	static FbxWriterTests()
	{
		string folder = Path.Combine(TestVariables.OutputSamplesFolder, "fbx");
		Directory.CreateDirectory(folder);

		TestCases.Add(new FbxFileModel(folder, FbxVersion.v6100, ContentType.ASCII));
		TestCases.Add(new FbxFileModel(folder, FbxVersion.v6100, ContentType.Binary));
		TestCases.Add(new FbxFileModel(folder, FbxVersion.v7700, ContentType.ASCII));
		TestCases.Add(new FbxFileModel(folder, FbxVersion.v7700, ContentType.Binary));
	}

	public FbxWriterTests(ITestOutputHelper output) : base(output)
	{
	}

	[Theory]
	[MemberData(nameof(TestCases))]
	public void WriteTest(FbxFileModel test)
	{
		FbxWriterOptions options = new FbxWriterOptions
		{
			ContentType = test.Content,
			Version = test.Version,
		};

		Scene scene = this.createScene();
		this.writeFile(test.Path, scene, options);
	}

	private Scene createScene()
	{
		Scene scene = new Scene();

		Node box = new Node("my_node");
		Mesh mesh = new Box("my_box").ToMesh();
		box.Entities.Add(mesh);

		scene.RootNode.Nodes.Add(box);

		return scene;
	}

	private void writeFile(string path, Scene scene, FbxWriterOptions options)
	{
		using (FbxWriter writer = new FbxWriter(path, scene, options))
		{
			writer.OnNotification += this.onNotification;
			writer.Write();
		}
	}
}