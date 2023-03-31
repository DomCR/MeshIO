using CSMath;
using MeshIO.Entities.Geometries.Layers;
using System.Collections.Generic;

namespace MeshIO.Entities.Geometries
{
    public class Geometry : Entity
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsVisible { get; set; }

        public bool CastShadows { get; set; }

        public bool ReceiveShadows { get; set; }

        public LayerCollection Layers { get; }

        public List<XYZ> Vertices { get; set; } = new List<XYZ>();

        public Geometry() : this(string.Empty) { }

        public Geometry(string name) : base(name)
        {
            this.Layers = new LayerCollection(this);
        }
    }
}
