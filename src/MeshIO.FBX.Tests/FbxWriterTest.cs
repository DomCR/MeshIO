using MeshIO.Entities.Geometries;
using MeshIO.Entities.Primitives;
using MeshIO.Tests.Shared;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.FBX.Tests
{
	public class FbxWriterTest : IOTestsBase
	{
		public static readonly TheoryData<FbxVersion> Versions = FbxTestCasesData.Versions;

		public FbxWriterTest(ITestOutputHelper output) : base(output) { }

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

			Scene scene = this.createScene();

			this.writeFile(path, scene, options);
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

			Scene scene = this.createScene();

			this.writeFile(path, scene, options);
		}

		private Scene createScene()
		{
			Scene scene = new Scene();

			Node box = new Node("my_node");
			Mesh mesh = new Box("my_box").CreateMesh();
			box.Entities.Add(mesh);

			scene.RootNode.Nodes.Add(box);

			return scene;
		}

		private void writeFile(string path, Scene scene, FbxWriterOptions options)
		{
			using (FbxWriter writer = new FbxWriter(path, scene, options))
			{
				writer.OnNotification += this.onNotification;
				writer.Write(options);
			}
		}
	}
}
