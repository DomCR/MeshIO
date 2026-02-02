using CSMath;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO.Entities.Geometries;

/// <summary>
/// Represents a geometric mesh composed of polygons, such as triangles or quadrilaterals.
/// </summary>
/// <remarks>A Mesh provides a collection of polygons and associated edges, allowing for the construction and
/// manipulation of complex geometric shapes. Polygons can be added using vertex sequences, and the mesh maintains
/// information about its constituent edges and polygons. This class is typically used in scenarios involving
/// computational geometry, graphics, or modeling where polygonal mesh structures are required.</remarks>
public class Mesh : Geometry
{
	/// <summary>
	/// Gets the collection of edge identifiers associated with the current instance.
	/// </summary>
	public List<int> Edges { get; } = new List<int>();

	/// <summary>
	/// Gets the collection of polygons contained in this instance.
	/// </summary>
	public List<Polygon> Polygons { get; } = new();

	/// <summary>
	/// Initializes a new instance of the Mesh class.
	/// </summary>
	public Mesh() : base() { }

	/// <summary>
	/// Initializes a new instance of the Mesh class with the specified name.
	/// </summary>
	/// <param name="name">The name to assign to the mesh.</param>
	public Mesh(string name) : base(name) { }

	/// <summary>
	/// Adds one or more polygons to the collection using the specified vertex sequences.
	/// </summary>
	/// <remarks>Each element in the parameter array is treated as a separate polygon. The method determines whether
	/// to add triangles or quadrilaterals based on the number of vertices in each sequence.</remarks>
	/// <param name="vertices">A parameter array of vertex sequences, where each sequence represents the vertices of a polygon. Each sequence must
	/// contain a number of vertices that is a multiple of 3 (for triangles) or 4 (for quadrilaterals).</param>
	/// <exception cref="ArgumentException">Thrown if any vertex sequence does not contain a number of vertices that is a multiple of 3 or 4.</exception>
	public void AddPolygons(params IEnumerable<XYZ> vertices)
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

	private void addQuads(IEnumerable<XYZ> vertices)
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

	private void addTriangles(IEnumerable<XYZ> vertices)
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
}