using CSMath;
using MeshIO.Entities.Geometries.Layers;
using System.Collections.Generic;

namespace MeshIO.Entities.Geometries
{
	public class Geometry : Entity
	{
		/// <summary>
		/// The geometry is visible or not
		/// </summary>
		public bool IsVisible { get; set; } = true;

		/// <summary>
		/// This geometry can cast shadow or not
		/// </summary>
		public bool CastShadows { get; set; } = true;

		/// <summary>
		/// This geometry can receive shadow or not
		/// </summary>
		public bool ReceiveShadows { get; set; } = true;

		public LayerCollection Layers { get; }

		public List<XYZ> Vertices { get; set; } = new List<XYZ>();

		public Geometry() : this(string.Empty) { }

		public Geometry(string name) : base(name)
		{
			this.Layers = new LayerCollection(this);
		}
	}
}
