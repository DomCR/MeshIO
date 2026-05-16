using CSMath;
using MeshIO.Entities.Geometries;
using System.Collections.Generic;

namespace MeshIO.Entities.Primitives;

/// <summary>
/// Represents a planar primitive defined by a center point, normal vector, length, and width.
/// </summary>
public class Plane : Primitive
{
	/// <summary>
	/// Gets or sets the center point of the plane.
	/// </summary>
	public XYZ Center { get; set; } = XYZ.Zero;

	/// <summary>
	/// Gets or sets the length of the plane along the local Z axis.
	/// </summary>
	public double Length { get; set; } = 1.0;

	/// <summary>
	/// Gets or sets the number of segments along the length of the plane.
	/// </summary>
	public int LengthSegments { get; set; } = 1;

	/// <summary>
	/// Gets or sets the normal vector of the plane.
	/// </summary>
	public XYZ Normal { get; set; } = XYZ.AxisY;

	/// <summary>
	/// Gets or sets the width of the plane along the local X axis.
	/// </summary>
	public double Width { get; set; } = 1.0;

	/// <summary>
	/// Gets or sets the number of segments along the width of the plane.
	/// </summary>
	public int WidthSegments { get; set; } = 1;

	/// <summary>
	/// Initializes a new instance of the <see cref="Plane"/> class with an empty name.
	/// </summary>
	public Plane() : this(string.Empty)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Plane"/> class with the specified name.
	/// </summary>
	/// <param name="name">The name of the plane.</param>
	public Plane(string name) : base(name)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Plane"/> class with the specified name, center, normal, length, and width.
	/// </summary>
	/// <param name="name">The name of the plane.</param>
	/// <param name="center">The center point of the plane.</param>
	/// <param name="normal">The normal vector of the plane.</param>
	/// <param name="length">The length of the plane.</param>
	/// <param name="width">The width of the plane.</param>
	public Plane(string name, XYZ center, XYZ normal, double length, double width) : base(name)
	{
		Center = center;
		Normal = normal;
		Length = length;
		Width = width;
	}

	/// <inheritdoc/>
	public override Mesh ToMesh()
	{
		List<XYZ> vertices = new List<XYZ>();
		List<XYZ> normals = new List<XYZ>();
		List<XY> uvs = new List<XY>();
		List<Quad> polygons = new List<Quad>();

		double lengthStep = Length / (double)LengthSegments;
		double widthStep = Width / (double)WidthSegments;
		double halfLength = Length * 0.5;
		double halfWidth = Width * 0.5;

		for (int i = 0; i <= LengthSegments; i++)
		{
			double z = i * lengthStep - halfLength;
			for (int j = 0; j <= WidthSegments; j++)
			{
				double x = j * widthStep - halfWidth;
				vertices.Add(new XYZ(x, 0.0, z));
				uvs.Add(new XY(
					(double)i / (double)LengthSegments,
					(double)j / (double)WidthSegments));

				if (i > 0 && j > 0)
				{
					int stride = LengthSegments + 1;
					polygons.Add(new Quad(
						i * stride + j - 1,
						i * stride + j,
						(i - 1) * stride + j,
						(i - 1) * stride + j - 1));
				}
			}
		}

		if (!Normal.Normalize().IsEqual(XYZ.AxisY))
		{
			Quaternion rotation = Quaternion.FromRotation(XYZ.AxisY, Normal.Normalize());
			for (int k = 0; k < vertices.Count; k++)
			{
				vertices[k] = rotation * vertices[k];
			}

			normals.Add(rotation * XYZ.AxisY);
		}
		else
		{
			normals.Add(XYZ.AxisY);
		}

		return this.createMesh(vertices, normals, uvs, polygons);
	}
}