using System.Collections;
using System.Collections.Generic;

namespace MeshIO.Entities.Geometries;

/// <summary>
/// Represents the base class for polygonal shapes defined by a sequence of integer vertices.
/// </summary>
/// <remarks>Polygon provides a common abstraction for polygons in integer coordinate space. Derived types must
/// implement the methods and properties to specify the number of vertices and their coordinates. This class implements
/// enumeration over the vertices and provides indexed access. Polygon is intended to be inherited by concrete polygon
/// implementations.</remarks>
public abstract class Polygon : IPolygon
{
	/// <inheritdoc/>
	public abstract int Dimension { get; }

	/// <inheritdoc/>
	public IEnumerator GetEnumerator()
	{
		return ToArray().GetEnumerator();
	}

	/// <inheritdoc/>
	IEnumerator<int> IEnumerable<int>.GetEnumerator()
	{
		return (IEnumerator<int>)ToArray().GetEnumerator();
	}

	/// <inheritdoc/>
	public abstract int[] ToArray();

	public int this[int index]
	{
		get
		{
			return this.ToArray()[index];
		}
	}
}