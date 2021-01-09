using System.Collections.Generic;

namespace MeshIO.Geometries.Primitives
{
	/// <summary>
	/// Defines the primitive meshes in a 3D environment.
	/// </summary>
	public interface IPrimitive
	{
		List<XYZ> ComputeVertices();
	}
}