using CSMath;
using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO.Entities.Primitives
{
	public class Box : Primitive
	{
		/// <summary>
		/// Gets or sets the center point of the object in 3D space.
		/// </summary>
		public XYZ Center { get; set; } = XYZ.Zero;

		/// <summary>
		/// Gets the length of the bounding box along the X-axis.
		/// </summary>
		public double LengthX { get; set; } = 1.0;

		/// <summary>
		/// Gets the length of the bounding box along the Y-axis.
		/// </summary>
		public double LengthY { get; set; } = 1.0;

		/// <summary>
		/// Gets the length of the bounding box along the Z-axis.
		/// </summary>
		public double LengthZ { get; set; } = 1.0;

		/// <summary>
		/// Initializes a new instance of the Box class with an empty label.
		/// </summary>
		public Box() : this(string.Empty) { }

		/// <summary>
		/// Initializes a new instance of the Box class with the specified name.
		/// </summary>
		/// <param name="name">The name to assign to the box. Cannot be null or empty.</param>
		public Box(string name) : base(name) { }

		/// <summary>
		/// Initializes a new instance of the Box class with the specified dimensions and center point.
		/// </summary>
		/// <param name="lengthX">The length of the box along the X-axis. Must be a positive value.</param>
		/// <param name="lengthY">The length of the box along the Y-axis. Must be a positive value.</param>
		/// <param name="lengthZ">The length of the box along the Z-axis. Must be a positive value.</param>
		/// <param name="center">The center point of the box, specified as an XYZ coordinate.</param>
		public Box(double lengthX, double lengthY, double lengthZ, XYZ center) : this()
		{
			this.LengthX = lengthX;
			this.LengthY = lengthY;
			this.LengthZ = lengthZ;
			this.Center = center;
		}

		/// <summary>
		/// Initializes a new instance of the Box class using the specified bounding box dimensions and center.
		/// </summary>
		/// <param name="boundingBox">The bounding box that provides the dimensions and center point for the new box. Cannot be null.</param>
		public Box(BoundingBox boundingBox)
			: this(boundingBox.LengthX, boundingBox.LengthY, boundingBox.LengthZ, boundingBox.Center)
		{
		}

		/// <inheritdoc/>
		/// <remarks>
		/// The current implementation returns a mesh with no shared vertices and the following layers:<br/>
		/// <see cref="LayerElementNormal"/><br/>
		/// <see cref="LayerElementUV"/><br/>
		/// configured with <see cref="MappingMode.ByVertex"/> and <see cref=" ReferenceMode.Direct"/>
		/// </remarks>
		public override Mesh ToMesh()
		{
			List<XYZ> vertices = new List<XYZ>();
			List<XYZ> normals = new List<XYZ>();
			List<XY> uvs = new List<XY>();
			List<Quad> polygons = new List<Quad>();

			int currQuad = 0;

			this.createFace(2, 1, 0, -1, -1, this.LengthZ, this.LengthY, this.LengthX, vertices, normals, uvs, polygons, ref currQuad);
			this.createFace(2, 1, 0, 1, -1, this.LengthZ, this.LengthY, 0.0 - this.LengthX, vertices, normals, uvs, polygons, ref currQuad);
			this.createFace(0, 2, 1, 1, 1, this.LengthX, this.LengthZ, this.LengthY, vertices, normals, uvs, polygons, ref currQuad);
			this.createFace(0, 2, 1, 1, -1, this.LengthX, this.LengthZ, 0.0 - this.LengthY, vertices, normals, uvs, polygons, ref currQuad);
			this.createFace(0, 1, 2, 1, -1, this.LengthX, this.LengthY, this.LengthZ, vertices, normals, uvs, polygons, ref currQuad);
			this.createFace(0, 1, 2, -1, -1, this.LengthX, this.LengthY, 0.0 - this.LengthZ, vertices, normals, uvs, polygons, ref currQuad);

			return this.createMesh(vertices, normals, uvs, polygons.Cast<Polygon>().ToList());
		}

		private void createFace(
			int index0, int index1, int index2,
			int dir1, int dir2,
			double length, double height, double width,
			List<XYZ> vertices, 
			List<XYZ> normals, 
			List<XY> uvs, 
			List<Quad> polygons,
			ref int currQuad)
		{
			//Creates a face with no shared vertices

			double faceLength = (double)(length / 2.0);
			double faceHeight = (double)(height / 2.0);
			double faceWidth = (double)(width / 2.0);

			double[] arr = new double[3];
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					arr[index0] = (double)(j * (double)length - faceLength) * dir1;
					arr[index1] = (double)(i * (double)(double)height - faceHeight) * dir2;
					arr[index2] = faceWidth;
					vertices.Add(new XYZ(arr[0], arr[1], arr[2]) + this.Center);
					arr[index0] = 0.0;
					arr[index1] = 0.0;
					arr[index2] = (width > 0.0) ? 1 : (-1);
					normals.Add(new XYZ(arr[0], arr[1], arr[2]));
					uvs.Add(new XY(j, 1 - i));
				}
			}

			int v = currQuad;
			int v2 = currQuad + 2;
			int v3 = currQuad + 3;
			int v4 = currQuad + 1;
			polygons.Add(new Quad(v, v2, v3, v4));

			currQuad += 4;
		}
	}
}