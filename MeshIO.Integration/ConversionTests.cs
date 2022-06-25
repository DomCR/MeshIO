using MeshIO.Core;
using MeshIO.Elements;
using MeshIO.GLTF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.Integration
{
	public class ConversionTests
	{
		private const string _samplesFolder = "../../../../samples/gltf";

		private const string _outFolder = "../../../../samples/out";

		public static readonly TheoryData<string> GlbFiles;

		private readonly ITestOutputHelper _output;

		static ConversionTests()
		{
			GlbFiles = new TheoryData<string>();

			foreach (string dir in Directory.GetDirectories(_samplesFolder))
			{
				string name = Path.GetFileName(dir);

				string glb = Path.Combine(dir, "glTF-Binary", $"{name}.glb");
				if (File.Exists(glb))
				{
					GlbFiles.Add(glb);
				}

			}
		}

		public ConversionTests(ITestOutputHelper output)
		{
			this._output = output;
		}

		[Theory]
		[MemberData(nameof(GlbFiles))]
		public void ReadGltfTest(string path)
		{
			Scene scene = null;

			using (GltfReader reader = new GltfReader(path))
			{
				reader.OnNotification = this.onNotification;

				scene = reader.Read();
			}

			FBX.FbxWriter.WriteAscii(Path.Combine(_outFolder,$"{Path.GetFileNameWithoutExtension(path)}.fbx"), scene);
		}

		private void onNotification(NotificationArgs e)
		{
			this._output.WriteLine(e.Message);
		}
	}
}
