using MeshIO.Formats.Obj;
using MeshIO.Tests.Common;
using MeshIO.Tests.TestModels;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.OBJ.Tests;

public class ObjReaderTest : IOTestsBase
{
	public static TheoryData<FileModel> ObjFiles { get; } = new();

	static ObjReaderTest()
	{
		loadSamples("obj", "obj", ObjFiles);
	}

	public ObjReaderTest(ITestOutputHelper output) : base(output) { }

	[Theory(Skip = "not implemented")]
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
