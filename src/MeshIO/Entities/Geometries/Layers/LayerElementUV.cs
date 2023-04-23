using CSMath;
using System.Collections.Generic;

namespace MeshIO.Entities.Geometries.Layers
{
    public class LayerElementUV : LayerElement
	{
		public List<XY> UV { get; set; } = new List<XY>();

		public LayerElementUV() : base() { }

		public LayerElementUV(MappingMode mappingMode, ReferenceMode referenceMode) : base(mappingMode, referenceMode) { }

		public void Add(XY uv)
		{
			this.UV.Add(uv);
		}

		public void AddRange(IEnumerable<XY> uvs)
		{
			foreach (var uv in uvs)
			{
				this.Add(uv);
			}
		}
	}
}
