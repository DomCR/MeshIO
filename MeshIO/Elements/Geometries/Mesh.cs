using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.Elements.Geometries
{
	public class Mesh : Geometry
	{
		public List<XYZ> Vertices { get; set; } = new List<XYZ>();
		public List<Polygon> Polygons { get; set; } = new List<Polygon>();
	}
}
