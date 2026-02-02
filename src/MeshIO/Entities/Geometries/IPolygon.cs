using System.Collections.Generic;

namespace MeshIO.Entities.Geometries;

/// <summary>
/// Defines the contract for a polygon represented by a sequence of integer vertex indices.
/// </summary>
/// <remarks>Implementations of this interface provide access to the vertices of a polygon and its dimensionality.
/// The interface supports enumeration of vertex indices and indexed access. The order of vertices is significant and
/// typically defines the shape and orientation of the polygon.</remarks>
public interface IPolygon : IEnumerable<int>
{
	/// <summary>
	/// Gets the number of dimension size of the polygon.
	/// </summary>
	public int Dimension { get; }

	/// <summary>
	/// Returns an array containing all elements in the collection.
	/// </summary>
	/// <returns>
	/// An array of integers that contains the elements of the collection.
	/// </returns>
	int[] ToArray();

	public int this[int index] { get; }
}
