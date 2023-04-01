using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO.Entities.Geometries.Layers
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

        public T GetLayer<T>()
            where T : LayerElement
        {
            return this._list.OfType<T>().FirstOrDefault();
        }

        public bool TryGetLayer<T>(out T layer)
            where T : LayerElement
        {
            layer = this._list.OfType<T>().FirstOrDefault();
            if (layer != null)
                return true;
            else
                return false;
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
