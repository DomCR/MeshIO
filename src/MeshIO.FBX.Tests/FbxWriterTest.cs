using CSMath;
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
			using (FbxWriter writer = new FbxWriter(new MemoryStream(), new Scene(), version))
			{
				writer.OnNotification += this.onNotification;
				writer.Write(FbxFileFormat.ASCII);
			}
		}

		[Theory]
		[MemberData(nameof(Versions))]
		public void WriteEmptyBinaryStream(FbxVersion version)
		{
			using (FbxWriter writer = new FbxWriter(new MemoryStream(), new Scene(), version))
			{
				writer.OnNotification += this.onNotification;
				writer.Write(FbxFileFormat.Binary);
			}
		}

		[Theory]
		[MemberData(nameof(Versions))]
		public void WriteFbxWithMesh(FbxVersion version)
		{
			string path = Path.Combine(FolderPath.OutFilesFbx, $"box_{version}_ascii.fbx");

			Scene scene = new Scene();

			Node box = new Node();
			box.Nodes.Add(new Box("my_box").CreateMesh());

			scene.RootNode.Nodes.Add(box);

			using (FbxWriter writer = new FbxWriter(path, scene, version))
			{
				writer.OnNotification += this.onNotification;
				writer.Write(FbxFileFormat.ASCII);
			}
		}
	}
}
