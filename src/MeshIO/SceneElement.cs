using System.Collections.Generic;

namespace MeshIO
{
	public abstract class SceneElement : Element3D
	{
		/// <summary>
		/// Scene where this element belongs to
		/// </summary>
		public Scene Scene { get; }

		public SceneElement() : base() { }

		public SceneElement(string name) : base(name) { }
	}
}
