namespace MeshIO.Entities.Geometries;

/// <summary>
/// Represents a polygon with three vertices, defined by their integer indices.
/// </summary>
/// <remarks>The Triangle class is typically used to represent a single triangle in a mesh or geometric structure,
/// where each index refers to a vertex in a shared vertex array. Indices should reference valid positions in the
/// associated vertex collection.</remarks>
public class Triangle : Polygon
{
	/// <inheritdoc/>
	public override int Dimension { get { return 3; } }

	public int Index0 { get; set; }

	public int Index1 { get; set; }

	public int Index2 { get; set; }

	public Triangle()
	{ }

	public Triangle(int i0, int i1, int i2)
	{
		Index0 = i0;
		Index1 = i1;
		Index2 = i2;
	}

	/// <summary>
	/// Initializes a new instance of the Triangle class using the specified array of vertex indices.
	/// </summary>
	/// <remarks>The values in the array are assigned to the <see cref="Index0"/>, <see cref="Index1"/>, and <see cref="Index2"/> properties, respectively.
	/// The caller is responsible for ensuring that the array contains valid vertex indices.</remarks>
	/// <param name="arr">An array containing exactly three integers representing the indices of the triangle's vertices. The array must not
	/// be null and must have a length of 3.</param>
	public Triangle(int[] arr)
	{
		Index0 = arr[0];
		Index1 = arr[1];
		Index2 = arr[2];
	}

	/// <inheritdoc/>
	public override int[] ToArray()
	{
		return new int[] { Index0, Index1, Index2 };
	}
}