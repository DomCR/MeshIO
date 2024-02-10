using CSMath;
using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using System;
using System.IO;
using Xunit;

namespace MeshIO.STL.Tests
{
	public class StlWriterTest
	{
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

			_mesh.AddPolygons(v1, v2, v3);
			_mesh.Layers.GetLayer<LayerElementNormal>().Normals.Add(new XYZ(0, 0, 1));
		}

		[Fact]
		public void WriteAsciiTest()
		{
			if (Environment.GetEnvironmentVariable("GITHUB_WORKFLOW") != null)
			{
				return;
			}

			string path = Path.Combine(FolderPath.OutFilesStl, "stl_ascii.stl");

			using (StlWriter wr = new StlWriter(path))
			{
				wr.WriteAscii(_mesh);
			}
		}

		[Fact]
		public void WriteBinaryTest()
		{
			if (Environment.GetEnvironmentVariable("GITHUB_WORKFLOW") != null)
			{
				return;
			}

			string path = Path.Combine(FolderPath.OutFilesStl, "stl_binary.stl");

			using (StlWriter wr = new StlWriter(path))
			{
				wr.WriteBinary(_mesh);
			}
		}
	}
}
