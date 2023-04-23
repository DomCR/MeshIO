using System;
using System.Collections.Generic;

namespace MeshIO.Entities.Geometries.Layers
{
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

		[Obsolete("delete",error: true)]
		public LayerElement(Geometry owner) : this()
		{
			this.Owner = owner;
		}
	}
}
