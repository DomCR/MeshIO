using MeshIO.Formats.Obj;
using MeshIO.Tests.Common;
using MeshIO.Tests.TestModels;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.Tests.Formats.Obj;

public class ObjReaderTests : IOTestsBase
{
	public static TheoryData<FileModel> ObjFiles { get; } = new();

	static ObjReaderTests()
	{
		loadSamples("obj", "obj", ObjFiles);
	}

	public ObjReaderTests(ITestOutputHelper output) : base(output) { }

	[Theory]
	[MemberData(nameof(ObjFiles))]
	public void ReadTest(FileModel test)
	{
		Scene scene = null;
		using (ObjReader reader = new ObjReader(test.Path))
		{
			reader.OnNotification += this.onNotification;
			scene = reader.Read();
		}

		Assert.NotNull(scene);
		Assert.NotEmpty(scene.RootNode.Nodes);
	}
}
