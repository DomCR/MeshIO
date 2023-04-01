using System.Collections.Generic;

namespace MeshIO
{
	public abstract class SceneElement : Element3D
	{
		/// <summary>
		/// Scene where this element belongs to
		/// </summary>
		public Scene Scene { get; }

		private readonly Dictionary<ulong, Element3D> _elements = new Dictionary<ulong, Element3D>();

		public SceneElement() : base() { }

		public SceneElement(string name) : base(name) { }
	}
}
