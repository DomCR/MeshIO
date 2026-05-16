using CSMath;
using MeshIO.Entities.Geometries;

namespace MeshIO.Entities.Primitives;

public class Sphere : Primitive
{
	public XYZ Center { get; set; } = XYZ.Zero;

	public double Radius { get; set; } = 1.0;

	public override Mesh ToMesh()
	{
		throw new System.NotImplementedException();
	}
}
