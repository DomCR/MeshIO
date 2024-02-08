using MeshIO.Tests.Shared;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.FBX.Tests
{
	public class FbxReaderTest : IOTestsBase
	{
		public static readonly TheoryData<string> AsciiFiles;

		public static readonly TheoryData<string> BinaryFiles;

		static FbxReaderTest()
		{
			AsciiFiles = new TheoryData<string>();
			foreach (string file in Directory.GetFiles(FolderPath.InFilesFbx, "*_ascii.fbx"))
			{
				AsciiFiles.Add(file);
			}

			BinaryFiles = new TheoryData<string>();
			foreach (string file in Directory.GetFiles(FolderPath.InFilesFbx, "*_binary.fbx"))
			{
				BinaryFiles.Add(file);
			}
		}

		public FbxReaderTest(ITestOutputHelper output) : base(output) { }

		[Theory]
		[MemberData(nameof(AsciiFiles))]
		[MemberData(nameof(BinaryFiles))]
		public void ParseTest(string test)
		{
			using (FbxReader reader = new FbxReader(test))
			{
				reader.OnNotification += onNotification;
				reader.Parse();
			}
		}

		[Theory]
		[MemberData(nameof(AsciiFiles))]
		public void ReadAsciiTest(string test)
		{
			readFile(test);
		}

		[Theory]
		[MemberData(nameof(BinaryFiles))]
		public void ReadBinaryTest(string test)
		{
			readFile(test);
		}

		private void readFile(string path)
		{
			Scene scene;

			using (FbxReader reader = new FbxReader(path))
			{
				reader.OnNotification += onNotification;
				scene = reader.Read();
			}

			Assert.NotNull(scene);
			Assert.NotNull(scene.RootNode);
			Assert.NotEmpty(scene.RootNode.Nodes);
		}
	}
}
