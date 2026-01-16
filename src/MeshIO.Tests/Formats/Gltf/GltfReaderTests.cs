using MeshIO.Formats.Gltf;
using MeshIO.Tests.Common;
using MeshIO.Tests.TestModels;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.Tests.Formats.Gltf;

public class GltfReaderTests : IOTestsBase
{
	public static TheoryData<FileModel> GlbFiles { get; } = new();

	public static TheoryData<FileModel> GltfFiles { get; } = new();

	static GltfReaderTests()
	{
		loadSamples("glb-gltf", "glb", GlbFiles);
		loadSamples("glb-gltf", "gltf", GltfFiles);
	}

	public GltfReaderTests(ITestOutputHelper output) : base(output)
	{
	}

	[Theory]
	[MemberData(nameof(GlbFiles))]
	public void ReadGlb(FileModel test)
	{
		using (GltfReader reader = new GltfReader(test.Path))
		{
			reader.OnNotification += this.onNotification;
			reader.Read();
		}
	}

	[Theory(Skip = "Gltf not implemented")]
	[MemberData(nameof(GltfFiles))]
	public void ReadGltf(FileModel test)
	{
		using (GltfReader reader = new GltfReader(test.Path))
		{
			reader.OnNotification += this.onNotification;
			reader.Read();
		}
	}
}