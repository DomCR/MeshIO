//Execute this test only if is in local and debug
#if DEBUG
using MeshIO.Tests.Shared;
using Xunit.Abstractions;

namespace MeshIO.GLTF.Tests
{
	public class GltfReaderLocalTest : IOTestsBase
	{
		public static readonly TheoryData<string> V1Files = new();

		public static readonly TheoryData<string, string> V2Files = new();

		private const string _samplesFolder = "..\\..\\..\\..\\..\\..\\glTF-Sample-Models";

		static GltfReaderLocalTest()
		{
			foreach (string file in Directory.GetFiles(Path.Combine(_samplesFolder, "1.0"), "*.glb", SearchOption.AllDirectories))
			{
				V1Files.Add(file);
			}

			foreach (string file in Directory.GetFiles(Path.Combine(_samplesFolder, "2.0"), "*.glb", SearchOption.AllDirectories))
			{
				V2Files.Add(Path.GetFileName(file), file);
			}
		}

		public GltfReaderLocalTest(ITestOutputHelper output) : base(output) { }

		[Theory]
		[MemberData(nameof(V1Files))]
		public void ReadGlbV1(string path)
		{
			using (GltfReader reader = new GltfReader(path))
			{
				reader.OnNotification += this.onNotification;
				Assert.Throws<NotSupportedException>(reader.Read);
			}
		}

		[Theory]
		[MemberData(nameof(V2Files))]
		public void ReadGlbV2(string file, string path)
		{
			using (GltfReader reader = new GltfReader(path))
			{
				reader.OnNotification += this.onNotification;
				reader.Read();
			}
		}
	}
}
#endif