using System.Collections.Generic;

namespace MeshIO.Entities
{
	/// <summary>
	/// Represents a 3D entity wich is attached to one or more nodes, entities allow instancing
	/// </summary>
	public abstract class Entity : SceneElement
	{
		/// <summary>
		/// Parents for this entity, multiple parents are possible to allow entity instancing
		/// </summary>
		public List<Node> ParentNodes { get; }

		public Entity() : base() { }

		public Entity(string name) : base(name) { }
	}
}
