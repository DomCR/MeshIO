using MeshIO.FBX.Readers.Parsers;
using MeshIO.Tests.Shared;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.FBX.Tests
{
	public class FbxIOTest : IOTestsBase
	{
		public FbxIOTest(ITestOutputHelper output) : base(output) { }

		[Fact]
		public void ParserTest()
		{
			string inPath = Path.Combine(FolderPath.InFilesFbx, "sample_basic_box_binary.fbx");

			FbxRootNode root = null;
			using (FbxBinaryParser parser = new FbxBinaryParser(File.OpenRead(inPath)))
			{
				root = parser.Parse();
			}

			using (FbxBinaryWriter writer = new FbxBinaryWriter(root, new MemoryStream()))
			{
				writer.Write();
			}
		}

		[Fact]
		public void IOBinaryTest()
		{
			string inPath = Path.Combine(FolderPath.InFilesFbx, "sample_basic_box_binary.fbx");

			Scene scene = null;
			using (FbxReader reader = new FbxReader(File.OpenRead(inPath)))
			{
				reader.OnNotification += this.onNotification;
				scene = reader.Read();
			}

			using (FbxWriter writer = new FbxWriter(new MemoryStream(), scene))
			{
				writer.Write();
			}
		}


		[Fact]
		public void IOAsciiTest()
		{
			string inPath = Path.Combine(FolderPath.InFilesFbx, "sample_basic_box_ascii.fbx");

			Scene scene = null;
			using (FbxReader reader = new FbxReader(File.OpenRead(inPath)))
			{
				reader.OnNotification += this.onNotification;
				scene = reader.Read();
			}

			using (FbxWriter writer = new FbxWriter(new MemoryStream(), scene))
			{
				writer.Write();
			}
		}
	}
}
