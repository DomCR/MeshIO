using CSMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.Elements.Geometries
{
	public class Mesh : Geometry
	{
		public List<int> Edges { get; set; } = new List<int>();

		public List<Polygon> Polygons { get; set; } = new List<Polygon>();

		public Mesh() : base() { }

		public Mesh(string name) : base(name) { }
	}
}
