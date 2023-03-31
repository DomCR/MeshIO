using System;
using System.Collections.Generic;

namespace MeshIO
{
	/// <summary>
	/// Base class for all the elements contained in the 3D environment
	/// </summary>
	public abstract class Element3D
	{
		/// <summary>
		/// Unique id to identify this element
		/// </summary>
		/// <remarks>
		/// If the Id doesn't have a value it means that is not attach to any scene
		/// </remarks>
		public ulong? Id { get; internal set; } = null;

		/// <summary>
		/// Name of the element
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Properties of this element
		/// </summary>
		public PropertyCollection Properties { get; }

		/// <summary>
		/// Default constructor
		/// </summary>
		public Element3D() : this(string.Empty) { }

		public Element3D(string name)
		{
			this.Name = name;
			this.Properties = new PropertyCollection(this);
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return $"{this.GetType().Name}:{this.Name}";
		}
	}

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

	public class Scene : SceneElement
	{
		public Node RootNode { get; } = new Node();

		public Scene() : base() { }

		public Scene(string name) : base(name) { }
	}

	public class Node : SceneElement
	{
		[Obsolete]
		public bool? MultiLayer { get; set; }
		[Obsolete]
		public bool? MultiTake { get; set; }
		[Obsolete]
		public bool Shading { get; set; } = true;
		[Obsolete]
		public string Culling { get; set; } = "CullingOff";

		public Transform Transform { get; internal set; } = new Transform();

		public Element3D Parent { get; }

		public List<Element3D> Children { get; }

		public Node() : base() { }

		public Node(string name) : base(name) { }
	}
}
