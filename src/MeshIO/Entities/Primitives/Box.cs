using CSMath;
using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO.Entities.Primitives
{
	public class Box : Primitive
	{
		public double XDimension { get; set; } = 1.0;

		public double YDimension { get; set; } = 1.0;

		public double ZDimension { get; set; } = 1.0;

		public XYZ Center { get; set; } = XYZ.Zero;

		public Box() : this(string.Empty) { }

		public Box(string name) : base(name) { }

		public Box(double xDimension, double yDimension, double zDimension, XYZ center) : this()
		{
			this.XDimension = xDimension;
			this.YDimension = yDimension;
			this.ZDimension = zDimension;
			this.Center = center;
		}

		/// <inheritdoc/>
		/// <remarks>
		/// The current implementation returns a mesh with no shared vertices and the following layers:<br/>
		/// <see cref="LayerElementNormal"/><br/>
		/// <see cref="LayerElementUV"/><br/>
		/// configured with <see cref="MappingMode.ByVertex"/> and <see cref=" ReferenceMode.Direct"/>
		/// </remarks>
		public override Mesh CreateMesh()
		{
			List<XYZ> vertices = new List<XYZ>();
			List<XYZ> normals = new List<XYZ>();
			List<XY> uvs = new List<XY>();
			List<Quad> polygons = new List<Quad>();

			int currQuad = 0;

			this.createFace(2, 1, 0, -1, -1, this.ZDimension, this.YDimension, this.XDimension, vertices, normals, uvs, polygons, ref currQuad);
			this.createFace(2, 1, 0, 1, -1, this.ZDimension, this.YDimension, 0.0 - this.XDimension, vertices, normals, uvs, polygons, ref currQuad);
			this.createFace(0, 2, 1, 1, 1, this.XDimension, this.ZDimension, this.YDimension, vertices, normals, uvs, polygons, ref currQuad);
			this.createFace(0, 2, 1, 1, -1, this.XDimension, this.ZDimension, 0.0 - this.YDimension, vertices, normals, uvs, polygons, ref currQuad);
			this.createFace(0, 1, 2, 1, -1, this.XDimension, this.YDimension, this.ZDimension, vertices, normals, uvs, polygons, ref currQuad);
			this.createFace(0, 1, 2, -1, -1, this.XDimension, this.YDimension, 0.0 - this.ZDimension, vertices, normals, uvs, polygons, ref currQuad);

			return this.createMesh(vertices, normals, uvs, polygons.Cast<Polygon>().ToList());
		}

		private void createFace(
			int index0, int index1, int index2,
			int dir1, int dir2,
			double length, double height, double width,
			List<XYZ> vertices, List<XYZ> normals, List<XY> uvs, List<Quad> polygons,
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
