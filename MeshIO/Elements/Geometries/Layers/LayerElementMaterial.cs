using System.Collections.Generic;

namespace MeshIO.Elements.Geometries.Layers
{
	public class LayerElementMaterial : LayerElement
	{
		public LayerElementMaterial(Geometry owner) : base(owner)
		{
			this.MappingInformationType = MappingMode.AllSame;
			this.ReferenceInformationType = ReferenceMode.IndexToDirect;

			this.Indices.Add(0);
		}
	}
}
