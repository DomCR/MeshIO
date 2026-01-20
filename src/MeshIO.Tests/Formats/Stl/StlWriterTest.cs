using MeshIO.Entities.Geometries;
using MeshIO.Entities.Primitives;
using MeshIO.Formats;
using MeshIO.Formats.Stl;
using System.IO;
using Xunit;

namespace MeshIO.Tests.Formats.Stl;

public class StlWriterTest
{
	[Theory]
	[InlineData(ContentType.ASCII)]
	[InlineData(ContentType.Binary)]
	public void WriteMeshTest(ContentType content)
	{
		Box box = new Box("my_box");
		Mesh mesh = box.ToMesh();

		StlWriter.WriteMesh(new MemoryStream(), mesh, new StlWriterOptions { ContentType = content });
	}
}