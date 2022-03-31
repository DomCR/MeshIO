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

		static StlWriterTest()
		{
			Path.Combine(_samplesFolder, "stl_ascii.stl");
			Path.Combine(_samplesFolder, "stl_binary.stl");
		}

		[Fact]
		public void WriteAsciiTest()
		{
			string path = Path.Combine(_samplesFolder, "stl_ascii.stl");

			Mesh mesh = new Mesh();
			mesh.Layers.Add(new LayerElementNormal
			{
				MappingMode = MappingMode.ByPolygon
			});

			using (StlWriter wr = new StlWriter(path))
			{
				wr.WriteAscii(mesh);
			}
		}

		[Fact]
		public void WriteBinaryTest()
		{
			string path = Path.Combine(_samplesFolder, "stl_binary.stl");

			Mesh mesh = new Mesh();
			mesh.Layers.Add(new LayerElementNormal 
			{
				MappingMode = MappingMode.ByPolygon
			});

			using (StlWriter wr = new StlWriter(path))
			{
				wr.WriteBinary(mesh);
			}
		}
	}
}
