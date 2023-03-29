using CSMath;
using MeshIO.Elements.Geometries.Layers;
using System.Collections.Generic;

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
