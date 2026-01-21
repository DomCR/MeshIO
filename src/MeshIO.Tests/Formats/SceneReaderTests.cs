using MeshIO.Formats;
using MeshIO.Tests.Common;
using MeshIO.Tests.TestModels;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.Tests.Formats
{
	public class SceneReaderTests : IOTestsBase
	{
		public static TheoryData<FileModel> InputCases { get; } = new();

		static SceneReaderTests()
		{
			loadSamples("stl", "stl", InputCases);
			loadSamples("fbx", "fbx", InputCases);
			loadSamples("glb", "glb", InputCases);
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
	}
}