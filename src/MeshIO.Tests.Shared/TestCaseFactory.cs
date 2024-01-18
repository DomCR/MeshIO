using MeshIO.Entities.Geometries;

namespace MeshIO.Tests.Shared
{
	public static class TestCaseFactory
	{
		public static Mesh CreateBox()
		{
			//TODO: complete test factory

			Mesh mesh = new Mesh("Box");
			mesh.Vertices.Add(new CSMath.XYZ(0, 0, 0));
			mesh.Vertices.Add(new CSMath.XYZ(10, 0, 0));
			mesh.Vertices.Add(new CSMath.XYZ(0, 10, 0));
			mesh.Vertices.Add(new CSMath.XYZ(10, 10, 0));

			mesh.Polygons.Add(new Triangle(0, 1, 2));

			return mesh;
		}

		public static Scene CreateSceneSimpleBox()
		{
			Scene scene = new Scene();

			return scene;
		}
	}
}
