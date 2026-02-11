using MeshIO.Formats;
using MeshIO.Tests.Common;
using MeshIO.Tests.TestModels;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.Tests.Formats;

public class SceneReaderTests : IOTestsBase
{
	public static TheoryData<FileModel> InputCases { get; } = new();
	public static TheoryData<FileModel> LocalCases { get; } = new();

	static SceneReaderTests()
	{
		loadSamples("stl", "stl", InputCases);
		loadSamples("fbx", "fbx", InputCases);
		loadSamples("glb", "glb", InputCases);

		loadSamples("../local/in/fbx", string.Empty, "fbx", LocalCases);
	}

	public SceneReaderTests(ITestOutputHelper output)
		: base(output)
	{
	}

	[Theory]
	[MemberData(nameof(InputCases))]
	public void ReadTest(FileModel test)
	{
		Scene scene;
		using (ISceneReader reader = FileFormat.GetReader(test.Path, onNotification))
		{
			scene = reader.Read();
		}
	}

	[Theory]
	[MemberData(nameof(LocalCases))]
	public void ReadLocalSamples(FileModel test)
	{
		if (string.IsNullOrEmpty(test.Path))
		{
			return;
		}

		Scene scene;
		using (ISceneReader reader = FileFormat.GetReader(test.Path, onNotification))
		{
			scene = reader.Read();
		}
	}
}