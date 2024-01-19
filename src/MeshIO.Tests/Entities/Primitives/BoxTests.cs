using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using MeshIO.Entities.Primitives;
using Xunit;

namespace MeshIO.Tests.Entities.Primitives
{
	public class BoxTests
	{
		[Fact]
		public void CreateDefault()
		{
			Box box = new Box();
			Mesh mesh = box.CreateMesh();

			Assert.NotEmpty(mesh.Vertices);
			Assert.Equal(24, mesh.Vertices.Count);
			Assert.Equal(mesh.Vertices.Count, mesh.Layers.GetLayer<LayerElementNormal>().Normals.Count);
			Assert.Equal(mesh.Vertices.Count, mesh.Layers.GetLayer<LayerElementUV>().UV.Count);
		}
	}
}
