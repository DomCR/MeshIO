using System.Collections.Generic;

namespace MeshIO;

/// <summary>
/// Represents a scene containing a hierarchical structure of nodes and <see cref="SubScenes"/>.
/// </summary>
/// <remarks>A scene serves as the root container for a collection of nodes, typically representing objects or
/// elements within a graphical or logical environment. Scenes can be composed hierarchically by including <see cref="SubScenes"/>,
/// allowing for modular organization and reuse. The root node provides the entry point to the node tree for this
/// scene.</remarks>
public class Scene : SceneElement
{
	/// <summary>
	/// Gets the root node of the tree structure.
	/// </summary>
	public Node RootNode { get; } = new Node();//TODO: Should it be a class for this one? (only nodes allowed)

	/// <summary>
	/// Gets the collection of sub-scenes contained within this scene.
	/// </summary>
	public List<Scene> SubScenes { get; } = new List<Scene>();

	public Scene() : base()
	{
	}

	public Scene(string name) : base(name)
	{
	}
}