namespace MeshIO
{
	public class Scene : SceneElement
	{
		public Node RootNode { get; } = new Node();

		public Scene() : base() { }

		public Scene(string name) : base(name) { }
	}
}
