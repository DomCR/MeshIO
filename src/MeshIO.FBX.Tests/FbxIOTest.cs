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
			//inPath = Path.Combine(FolderPath.InFilesFbx, "sample_basic_box_ascii_clean.fbx");

			FbxRootNode root = null;
			using (FbxBinaryParser parser = new FbxBinaryParser(File.OpenRead(inPath)))
			{
				root = parser.Parse();
			}

			using (FbxBinaryWriter writer = new FbxBinaryWriter(root, File.Create(Path.Combine(FolderPath.OutFilesFbx, $"io_bin.fbx"))))
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
				scene = reader.Read();
			}

			//using (FbxWriter writer = new FbxWriter(File.Create(Path.Combine(FolderPath.OutFilesFbx, $"io_bin_writer.fbx")), scene))
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
				scene = reader.Read();
			}

			//using (FbxWriter writer = new FbxWriter(File.Create(Path.Combine(FolderPath.OutFilesFbx, $"io_bin_writer.fbx")), scene))
			using (FbxWriter writer = new FbxWriter(new MemoryStream(), scene))
			{
				writer.Write();
			}
		}
	}
}
