using MeshIO.Entities.Geometries;
using MeshIO.Entities.Primitives;
using MeshIO.FBX.Readers.Parsers;
using MeshIO.Tests.Shared;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.FBX.Tests
{
	public class FbxWriterTest : IOTestsBase
	{
		public static readonly TheoryData<FbxVersion> Versions = FbxTestCasesData.Versions;

		public FbxWriterTest(ITestOutputHelper output) : base(output) { }

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
		public void IOTest()
		{
			string inPath = Path.Combine(FolderPath.InFilesFbx, "sample_basic_box_binary.fbx");
			//inPath = Path.Combine(FolderPath.InFilesFbx, "sample_basic_box_ascii_clean.fbx");

			Scene scene = null;
			using (FbxReader reader = new FbxReader(File.OpenRead(inPath)))
			{
				scene = reader.Read();
			}

			using (FbxWriter writer = new FbxWriter(File.Create(Path.Combine(FolderPath.OutFilesFbx, $"io_bin_writer.fbx")), scene))
			{
				writer.Write();
			}
		}

		[Theory]
		[MemberData(nameof(Versions))]
		public void WriteEmptyAsciiStream(FbxVersion version)
		{
			FbxWriterOptions options = new FbxWriterOptions
			{
				IsBinaryFormat = false,
				Version = version,
			};

			using (FbxWriter writer = new FbxWriter(new MemoryStream(), new Scene(), options))
			{
				writer.OnNotification += this.onNotification;
				writer.Write(new FbxWriterOptions() { IsBinaryFormat = false });
			}
		}

		[Theory]
		[MemberData(nameof(Versions))]
		public void WriteAsciiFbxWithMesh(FbxVersion version)
		{
			FbxWriterOptions options = new FbxWriterOptions
			{
				IsBinaryFormat = false,
				Version = version,
			};

			string path = Path.Combine(FolderPath.OutFilesFbx, $"box_{version}_ascii.fbx");

			Scene scene = new Scene();

			Node box = new Node("my_node");
			Mesh mesh = new Box("my_box").CreateMesh();
			box.Entities.Add(mesh);

			scene.RootNode.Nodes.Add(box);

			using (FbxWriter writer = new FbxWriter(path, scene, options))
			{
				writer.OnNotification += this.onNotification;
				writer.Write(new FbxWriterOptions() { IsBinaryFormat = false });
			}
		}

		[Theory]
		[MemberData(nameof(Versions))]
		public void WriteEmptyBinaryStream(FbxVersion version)
		{
			FbxWriterOptions options = new FbxWriterOptions
			{
				IsBinaryFormat = true,
				Version = version,
			};

			using (FbxWriter writer = new FbxWriter(new MemoryStream(), new Scene()))
			{
				writer.OnNotification += this.onNotification;
				writer.Write(new FbxWriterOptions() { IsBinaryFormat = true });
			}
		}

		[Theory]
		[MemberData(nameof(Versions))]
		public void WriteBinaryFbxWithMesh(FbxVersion version)
		{
			FbxWriterOptions options = new FbxWriterOptions
			{
				IsBinaryFormat = true,
				Version = version,
			};

			string path = Path.Combine(FolderPath.OutFilesFbx, $"box_{version}_binary.fbx");

			Scene scene = new Scene();

			Node box = new Node("my_node");
			Mesh mesh = new Box("my_box").CreateMesh();
			box.Entities.Add(mesh);

			scene.RootNode.Nodes.Add(box);

			using (FbxWriter writer = new FbxWriter(path, scene, options))
			{
				writer.OnNotification += this.onNotification;
				writer.Write(options);
			}

			using (FbxReader reader = new FbxReader(path))
			{
				var s = reader.Read();
			}
		}
	}
}
