using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using MeshIO.Formats.Fbx;
using MeshIO.Tests.Common;
using MeshIO.Tests.TestModels;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.Tests.Formats.Fbx;

public class FbxReaderTests : IOTestsBase
{
	public static TheoryData<FileModel> FbxFiles { get; } = new();

	static FbxReaderTests()
	{
		loadSamples("fbx", "fbx", FbxFiles);
	}

	public FbxReaderTests(ITestOutputHelper output) : base(output)
	{
	}

	[Theory]
	[MemberData(nameof(FbxFiles))]
	public void ParseTest(FileModel test)
	{
		FbxRootNode root = null;
		using (FbxReader reader = new FbxReader(test.Path))
		{
			reader.OnNotification += onNotification;
			root = reader.Parse();
		}
	}

	[Theory]
	[MemberData(nameof(FbxFiles))]
	public void ReadTest(FileModel test)
	{
		Scene scene = null;
		FbxVersion version;
		using (FbxReader reader = new FbxReader(test.Path))
		{
			reader.OnNotification += this.onNotification;
			scene = reader.Read();
			version = reader.Version;
		}

		Assert.NotNull(scene.RootNode);
		Assert.NotEmpty(scene.RootNode.Nodes);

		var n = scene.RootNode.Nodes.FirstOrDefault(n => n.Name == "default_cube");
		Mesh mesh = n.Entities.OfType<Mesh>().FirstOrDefault();
		this.assertCube(mesh);
	}

	private void assertCube(Mesh mesh)
	{
		Assert.NotNull(mesh);
		Assert.NotEmpty(mesh.Polygons);
		Assert.NotEmpty(mesh.Vertices);
		Assert.NotEmpty(mesh.Edges);
		Assert.NotEmpty(mesh.Layers);
		var normalLayer = mesh.Layers.GetLayer<LayerElementNormal>();
		Assert.NotNull(normalLayer);
		Assert.NotEmpty(normalLayer.Normals);
	}
}