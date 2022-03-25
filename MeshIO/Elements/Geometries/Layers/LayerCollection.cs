using System;
using System.Collections;
using System.Collections.Generic;

namespace MeshIO.Elements.Geometries.Layers
{
	public class LayerCollection : IEnumerable<LayerElement>   //TODO: Organize and create different layers for a geometry element
	{
		public Geometry Owner { get; }

		private List<LayerElement> _list = new List<LayerElement>();

		public LayerCollection(Geometry geometry)
		{
			this.Owner = geometry;
		}

		public void Add(LayerElement layerElement)
		{
			if (layerElement.Owner != null)
				throw new ArgumentException($"The layer element already has an owner {this.Owner.Name}", nameof(layerElement));

			layerElement.Owner = this.Owner;
			this._list.Add(layerElement);
		}

		public IEnumerator<LayerElement> GetEnumerator()
		{
			return this._list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._list.GetEnumerator();
		}
	}
}
