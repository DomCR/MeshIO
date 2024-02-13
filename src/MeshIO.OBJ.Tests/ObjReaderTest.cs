using MeshIO.Tests.Shared;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.OBJ.Tests
{
	public class ObjReaderTest : IOTestsBase
	{
		public static readonly TheoryData<string> Files;

		static ObjReaderTest()
		{
			Files = new TheoryData<string>();
			foreach (string file in Directory.GetFiles(FolderPath.InFilesObj, "*.obj"))
			{
				Files.Add(file);
			}
		}

		public ObjReaderTest(ITestOutputHelper output) : base(output) { }

		[Theory]
		[MemberData(nameof(Files))]
		public void ReadTest(string test)
		{
			Scene scene = null;
			using (ObjReader reader = new ObjReader(test))
			{
				reader.OnNotification += this.onNotification;
				scene = reader.Read();
			}

			Assert.NotNull(scene);
			Assert.NotEmpty(scene.RootNode.Nodes);
		}
	}
}
