using CSMath;
using MeshIO.Elements.Geometries;
using MeshIO.Elements.Geometries.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.Elements.Geometries
{
	public class Geometry : Element
	{
		public LayerCollection Layers { get; }

		public List<XYZ> Vertices { get; set; } = new List<XYZ>();

		public Geometry(string name) : base(name)
		{
			this.Layers = new LayerCollection(this);
		}

		public Geometry() : this(string.Empty) { }
	}
}
