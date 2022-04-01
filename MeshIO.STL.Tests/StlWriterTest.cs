using CSMath;
using MeshIO.Elements.Geometries;
using MeshIO.Elements.Geometries.Layers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MeshIO.STL.Tests
{
	public class StlWriterTest
	{
		private const string _samplesFolder = "../../../../samples/out";

		private static readonly Mesh _mesh;

		static StlWriterTest()
		{
			_mesh = new Mesh("test_box");
			_mesh.Layers.Add(new LayerElementNormal
			{
				MappingMode = MappingMode.ByPolygon
			});

			XYZ v1 = new XYZ(0, 0, 0);
			XYZ v2 = new XYZ(0, 1, 0);
			XYZ v3 = new XYZ(1, 1, 0);

			_mesh.AddTriangles(v1, v2, v3);
			_mesh.Layers.GetLayer<LayerElementNormal>().Normals.Add(new XYZ(0, 0, 1));
		}

		[Fact]
		public void WriteAsciiTest()
		{
			if (Environment.GetEnvironmentVariable("GITHUB_WORKFLOW") == "BUILD_TEST")
			{
				return;
			}

			string path = Path.Combine(_samplesFolder, "stl_ascii.stl");

			using (StlWriter wr = new StlWriter(path))
			{
				wr.WriteAscii(_mesh);
			}
		}

		[Fact]
		public void WriteBinaryTest()
		{
			if (Environment.GetEnvironmentVariable("GITHUB_WORKFLOW") == "BUILD_TEST")
			{
				return;
			}

			string path = Path.Combine(_samplesFolder, "stl_binary.stl");

			using (StlWriter wr = new StlWriter(path))
			{
				wr.WriteBinary(_mesh);
			}
		}
	}
}
