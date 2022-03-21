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
		
		public MappingMode MappingInformationType { get; set; }
		
		public ReferenceMode ReferenceInformationType { get; set; }

		public List<int> Indices { get; } = new List<int>();

		protected Geometry _owner;

		public LayerElement(Geometry owner)
		{
			this._owner = owner;
		}
	}
}
