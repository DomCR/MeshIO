namespace MeshIO
{
	/// <summary>
	/// Base class for objects that can be in a scene
	/// </summary>
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
