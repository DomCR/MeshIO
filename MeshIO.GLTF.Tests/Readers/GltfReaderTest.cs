using MeshIO.Core;
using MeshIO.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.GLTF.Tests.Readers
{
	public class GltfReaderTest
	{
		private const string _samplesFolder = "../../../../samples/gltf";

		public static readonly TheoryData<string> GltfFiles;

		public static readonly TheoryData<string> GlbFiles;

		public static readonly TheoryData<string> DracoFiles;

		public static readonly TheoryData<string> EmbeddedFiles;

		private readonly ITestOutputHelper _output;

		static GltfReaderTest()
		{
			GltfFiles = new TheoryData<string>();
			GlbFiles = new TheoryData<string>();
			DracoFiles = new TheoryData<string>();
			EmbeddedFiles = new TheoryData<string>();

			foreach (string dir in Directory.GetDirectories(_samplesFolder))
			{
				string name = Path.GetFileName(dir);

				string gltf = Path.Combine(dir, "glTF", $"{name}.gltf");
				if (File.Exists(gltf))
				{
					GltfFiles.Add(gltf);
				}

				string glb = Path.Combine(dir, "glTF-Binary", $"{name}.glb");
				if (File.Exists(glb))
				{
					GlbFiles.Add(glb);
				}


				string draco = Path.Combine(dir, "glTF-Draco", $"{name}.gltf");
				if (File.Exists(draco))
				{
					DracoFiles.Add(draco);
				}

				string embedded = Path.Combine(dir, "glTF-Embedded", $"{name}.gltf");
				if (File.Exists(embedded))
				{
					EmbeddedFiles.Add(embedded);
				}
			}
		}

		public GltfReaderTest(ITestOutputHelper output)
		{
			this._output = output;
		}

		[Theory]
		[MemberData(nameof(GltfFiles))]
		public void ReadGltfTest(string test)
		{
			this.readFile(test);
		}

		[Theory]
		[MemberData(nameof(GlbFiles))]
		public void ReadGlbTest(string test)
		{
			this.readFile(test);
		}

		[Theory]
		[MemberData(nameof(DracoFiles))]
		public void ReadDracoTest(string test)
		{
			this.readFile(test);
		}

		[Theory]
		[MemberData(nameof(EmbeddedFiles))]
		public void ReadEmbeddedTest(string test)
		{
			this.readFile(test);
		}

		private Scene readFile(string path)
		{
			using (GltfReader reader = new GltfReader(path))
			{
				return reader.Read();
			}
		}

		private void onNotification(NotificationArgs e)
		{
			this._output.WriteLine(e.Message);
		}
	}
}
