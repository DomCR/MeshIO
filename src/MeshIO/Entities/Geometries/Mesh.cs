using CSMath;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO.Entities.Geometries
{
	public class Mesh : Geometry
	{
		public List<int> Edges { get; set; } = new List<int>();

		public List<Polygon> Polygons { get; set; } = new List<Polygon>();

		public Mesh() : base() { }

		public Mesh(string name) : base(name) { }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="vertices"></param>
		/// <exception cref="ArgumentException"></exception>
		public void AddPolygons(params XYZ[] vertices)
		{
			int count = vertices.Count();
			if (vertices.Count() % 3 == 0)
			{
				this.addTriangles(vertices);
			}
			else if (vertices.Count() % 4 == 0)
			{
				this.addQuads(vertices);
			}
			else
			{
				throw new ArgumentException("The array of vertices should be multiple of 3 or 4", nameof(vertices));
			}
		}

		private void addTriangles(XYZ[] vertices)
		{
			if (this.Polygons.Any() && this.Polygons.First().GetType() != typeof(Triangle))
				throw new ArgumentException("This mesh is not formed by Triangles");

			for (int i = 0; i < vertices.Count(); i += 3)
			{
				this.Vertices.Add(vertices.ElementAt(i));
				this.Vertices.Add(vertices.ElementAt(i + 1));
				this.Vertices.Add(vertices.ElementAt(i + 2));

				this.Polygons.Add(new Triangle(
						this.Vertices.Count - 3,
						this.Vertices.Count - 2,
						this.Vertices.Count - 1
					));
			}
		}

		private void addQuads(XYZ[] vertices)
		{
			if (this.Polygons.Any() && this.Polygons.First().GetType() != typeof(Quad))
				throw new ArgumentException("This mesh is not formed by Quads");

			for (int i = 0; i < vertices.Count(); i += 4)
			{
				this.Vertices.Add(vertices.ElementAt(i));
				this.Vertices.Add(vertices.ElementAt(i + 1));
				this.Vertices.Add(vertices.ElementAt(i + 2));
				this.Vertices.Add(vertices.ElementAt(i + 3));

				this.Polygons.Add(new Quad(
						this.Vertices.Count - 4,
						this.Vertices.Count - 3,
						this.Vertices.Count - 2,
						this.Vertices.Count - 1
					));
			}
		}
	}
}
