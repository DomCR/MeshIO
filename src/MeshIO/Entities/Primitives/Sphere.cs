using CSMath;
using MeshIO.Entities.Geometries;
using System;
using System.Collections.Generic;

namespace MeshIO.Entities.Primitives;

/// <summary>
/// Represents a three-dimensional sphere primitive defined by its center, radius, and segmentation parameters.
/// </summary>
/// <remarks>The Sphere class provides properties to control the sphere's tessellation, including the number of
/// width and height segments, as well as angular ranges for partial spheres. Adjusting these parameters affects the
/// level of detail and the portion of the sphere that is generated. The sphere is centered at the specified Center
/// point and can be used to generate a mesh representation for rendering or geometric processing.</remarks>
public class Sphere : Primitive
{
	/// <summary>
	/// Gets or sets the center point of the object in 3D space.
	/// </summary>
	public XYZ Center { get; set; } = XYZ.Zero;

	/// <summary>
	/// Gets or sets the number of vertical segments used to construct the geometry.
	/// </summary>
	/// <remarks>The value must be at least 2. Increasing the number of height segments can improve the smoothness
	/// of the geometry but may impact performance.</remarks>
	public int HeightSegments { get; set; } = 16;

	/// <summary>
	/// Gets or sets the length of the phi angle, in radians.
	/// </summary>
	public double PhiLength { get; set; } = MathHelper.TwoPI;

	/// <summary>
	/// Gets or sets the starting angle, in radians, for the phi coordinate.
	/// </summary>
	public double PhiStart { get; set; }

	/// <summary>
	/// Gets or sets the radius of the shape.
	/// </summary>
	public double Radius { get; set; } = 1.0;

	/// <summary>
	/// Gets or sets the central angle, in radians, that defines the arc length of the shape.
	/// </summary>
	/// <remarks>The value typically ranges from 0 to 2π. Adjusting this property changes the portion of the full
	/// circle represented by the shape.</remarks>
	public double ThetaLength { get; set; } = Math.PI;

	/// <summary>
	/// Gets or sets the starting angle, in radians, for the operation or calculation.
	/// </summary>
	public double ThetaStart { get; set; }

	/// <summary>
	/// Gets or sets the number of segments used to subdivide the width of the geometry.
	/// </summary>
	/// <remarks>Increasing the number of width segments can improve the smoothness of the geometry at the cost of
	/// additional processing and rendering overhead.</remarks>
	public int WidthSegments { get; set; } = 32;

	/// <summary>
	/// Initializes a new instance of the Sphere class with default values.
	/// </summary>
	/// <remarks>This constructor creates a Sphere with an empty name. Use this overload when no initial name is
	/// required.</remarks>
	public Sphere() : this(string.Empty)
	{
	}

	/// <summary>
	/// Initializes a new instance of the Sphere class with the specified name.
	/// </summary>
	/// <param name="name">The name to assign to the sphere. Cannot be null or empty.</param>
	public Sphere(string name) : base(name)
	{
	}

	/// <inheritdoc/>
	public override Mesh ToMesh()
	{
		int index = 0;
		int totalVertices = (WidthSegments + 1) * (HeightSegments + 1);
		List<XYZ> vertices = new List<XYZ>();
		List<XYZ> normals = new List<XYZ>();
		List<XY> uvs = new List<XY>();
		List<Triangle> polygons = new();

		double thetaEnd = (double)(double)ThetaStart + (double)(double)ThetaLength;

		var indexGrid = new int[HeightSegments + 1][];

		for (int i = 0; i <= HeightSegments; i++)
		{
			var row = new int[WidthSegments + 1];
			double v = (double)i / HeightSegments;

			for (int j = 0; j <= WidthSegments; j++)
			{
				double u = (double)j / (double)WidthSegments;
				double x = -(double)(double)Radius * Math.Cos((double)(double)PhiStart + u * (double)(double)PhiLength) * Math.Sin((double)(double)ThetaStart + v * (double)(double)ThetaLength);
				double y = (double)(double)Radius * Math.Cos((double)(double)ThetaStart + v * (double)(double)ThetaLength);
				double z = (double)(double)Radius * Math.Sin((double)(double)PhiStart + u * (double)(double)PhiLength) * Math.Sin((double)(double)ThetaStart + v * (double)(double)ThetaLength);

				vertices.Add(new XYZ(x, y, z) + this.Center);
				normals.Add(new XYZ(x, y, z).Normalize());
				uvs.Add(new XY(u, 1f - v));
				row[j] = index;
				index++;
			}

			indexGrid[i] = row;
		}

		for (int k = 0; k < HeightSegments; k++)
		{
			for (int l = 0; l < WidthSegments; l++)
			{
				int a = indexGrid[k][l + 1];
				int b = indexGrid[k][l];
				int c = indexGrid[k + 1][l];
				int d = indexGrid[k + 1][l + 1];

				bool notTopPole = k != 0 || (double)(double)ThetaStart > 0f;
				bool notBottomPole = k != HeightSegments - 1 || (double)thetaEnd < Math.PI;

				if (notTopPole && notBottomPole)
				{
					var q = new Quad(a, b, c, d);
					polygons.AddRange(q.ToTriangles());
				}
				else if (notTopPole)
				{
					polygons.Add(new Triangle(a, b, d));
				}
				else if (notBottomPole)
				{
					polygons.Add(new Triangle(b, c, d));
				}
			}
		}

		return this.createMesh(vertices, normals, uvs, polygons);
	}
}