using CSMath;
using MeshIO.Elements.Geometries.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.Elements.Geometries.Layers
{
	//TODO: finish the layer element implementations
	public abstract class LayerElement
	{
		/// <summary>
		/// Name of the layer
		/// </summary>
		public string Name { get; set; } = string.Empty;

		public MappingMode MappingMode { get; set; }

		public ReferenceMode ReferenceMode { get; set; }

		public List<int> Indices { get; } = new List<int>();

		public Geometry Owner { get; internal set; }

		public LayerElement()
		{
			this.MappingMode = MappingMode.AllSame;
			this.ReferenceMode = ReferenceMode.IndexToDirect;
		}

		public virtual LayerElement Clone()
		{
			throw new NotImplementedException();
		}
	}
}
