using System.Collections.Generic;

namespace MeshIO
{
	public class Scene : SceneElement
	{
		/// <summary>
		/// Root node of the scene
		/// </summary>
		public Node RootNode { get; } = new Node();

		private readonly Dictionary<ulong, Element3D> _elements = new Dictionary<ulong, Element3D>();

		public Scene() : base() { }

		public Scene(string name) : base(name) { }
	}
}
