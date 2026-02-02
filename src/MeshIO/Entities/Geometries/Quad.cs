using System.Collections.Generic;

namespace MeshIO.Entities.Geometries;

/// <summary>
/// Represents a quadrilateral polygon defined by four vertex indices.
/// </summary>
/// <remarks>A Quad is a specific type of Polygon with exactly four vertices. The vertex indices typically refer
/// to positions in an external vertex array or mesh structure. This class provides convenient access to each index and
/// methods to convert the quad into triangles for rendering or processing purposes.</remarks>
public class Quad : Polygon
{
	/// <inheritdoc/>
	public override int Dimension { get { return 4; } }

	public int Index0 { get; set; }

	public int Index1 { get; set; }

	public int Index2 { get; set; }

	public int Index3 { get; set; }

	public Quad(int i0, int i1, int i2, int i3)
	{
		Index0 = i0;
		Index1 = i1;
		Index2 = i2;
		Index3 = i3;
	}

	/// <summary>
	/// Initializes a new instance of the Quad class using the specified array of indices.
	/// </summary>
	/// <remarks>The values from the array are assigned in order to <see cref="Index0"/>, <see cref="Index1"/>, <see cref="Index2"/>, and <see cref="Index3"/>. The caller
	/// must ensure that the array contains exactly four elements.</remarks>
	/// <param name="arr">An array of four integers representing the indices to assign to the quad. The array must have a length of 4.</param>
	public Quad(int[] arr)
	{
		Index0 = arr[0];
		Index1 = arr[1];
		Index2 = arr[2];
		Index3 = arr[3];
	}

	/// <inheritdoc/>
	public override int[] ToArray()
	{
		return new int[] { Index0, Index1, Index2, Index3 };
	}

	/// <summary>
	/// Converts the quadrilateral defined by this instance into a collection of triangles.
	/// </summary>
	/// <remarks>The returned triangles are constructed by splitting the quadrilateral along one of its diagonals.
	/// The order of the triangles corresponds to the order of the indices used in the quadrilateral definition.</remarks>
	/// <returns>An enumerable collection containing two triangles that together represent the original quadrilateral.</returns>
	public IEnumerable<Triangle> ToTriangles()
	{
		List<Triangle> list = new List<Triangle>();
		list.Add(new Triangle(Index0, Index1, Index2));
		list.Add(new Triangle(Index2, Index3, Index0));
		return list;
	}
}