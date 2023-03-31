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

        [Obsolete]
        public void AddTriangles(params XYZ[] vertices)
        {
            if (vertices.Count() % 3 != 0)
                throw new ArgumentException("The array of vertices should be multiple of 3", nameof(vertices));

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
}
