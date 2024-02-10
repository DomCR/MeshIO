using MeshIO.Entities;
using MeshIO.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO
{
	/// <summary>
	/// Scene node
	/// </summary>
	public class Node : SceneElement
	{
		/// <summary>
		/// The node and all the components are visible or not
		/// </summary>
		public bool IsVisible { get; set; } = true;

		/// <summary>
		/// Get the local transform for this node
		/// </summary>
		public Transform Transform { get; internal set; } = new Transform();

		/// <summary>
		/// Get the parent for this node
		/// </summary>
		public Element3D Parent { get; }

		/// <summary>
		/// Nested nodes
		/// </summary>
		public List<Node> Nodes { get; } = new();

		/// <summary>
		/// Gets the default material for this node
		/// </summary>
		public Material Material
		{
			get
			{
				return this.Materials.FirstOrDefault();
			}
		}

		/// <summary>
		/// Materials attached to this node
		/// </summary>
		public List<Material> Materials { get; } = new();

		/// <summary>
		/// Gets the defining entity for this node
		/// </summary>
		public Entity Entity
		{
			get
			{
				return this.Entities.FirstOrDefault();
			}
		}

		/// <summary>
		/// Entities attached to this node
		/// </summary>
		public List<Entity> Entities { get; } = new();

		/// <summary>
		/// Default constructor
		/// </summary>
		public Node() : this(string.Empty) { }

		public Node(string name) : base(name) { }

		/// <summary>
		/// Add an <see cref="Element3D"/> to this node structure
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="element"></param>
		/// <exception cref="ArgumentException"></exception>
		public void Add<T>(T element)
			where T : Element3D
		{
			switch (element)
			{
				case Entity e:
					this.Entities.Add(e);
					break;
				case Material m:
					this.Materials.Add(m);
					break;
				case Node n:
					this.Nodes.Add(n);
					break;
				default:
					throw new ArgumentException("", nameof(element));
			}
		}

		/// <summary>
		/// Get the global transformation for this node
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public Transform GetGlobalTransform()
		{
			throw new NotImplementedException();
		}
	}
}
