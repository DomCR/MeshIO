using MeshIO.Tests.Shared;
using Xunit.Abstractions;

namespace MeshIO.GLTF.Tests
{
	public class GltfReaderTests : IOTestsBase
	{
		public static readonly TheoryData<string> GlbFiles;

		public static readonly TheoryData<string> GltfFiles;

		static GltfReaderTests()
		{
			GlbFiles = new TheoryData<string>();
			foreach (string file in Directory.GetFiles(FolderPath.InFilesGltf, "*.glb"))
			{
				GlbFiles.Add(file);
			}

			GltfFiles = new TheoryData<string>();
			foreach (string file in Directory.GetFiles(FolderPath.InFilesGltf, "*.gltf"))
			{
				GltfFiles.Add(file);
			}
		}

		public GltfReaderTests(ITestOutputHelper output) : base(output) { }

		[Theory]
		[MemberData(nameof(GlbFiles))]
		public void ReadGlb(string path)
		{
			using (GltfReader reader = new GltfReader(path))
			{
				reader.OnNotification += this.onNotification;
				reader.Read();
			}
		}

		[Theory]
		[MemberData(nameof(GltfFiles))]
		public void ReadGltf(string path)
		{
			using (GltfReader reader = new GltfReader(path))
			{
				reader.OnNotification += this.onNotification;
				reader.Read();
			}
		}
	}
}