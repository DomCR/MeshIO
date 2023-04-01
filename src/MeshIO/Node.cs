using System;
using System.Collections.Generic;

namespace MeshIO
{
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

		/// <summary>
		/// Get the local transform for this node
		/// </summary>
		public Transform Transform { get; internal set; } = new Transform();

		/// <summary>
		/// Get the parent for this node
		/// </summary>
		public Element3D Parent { get; }

		/// <summary>
		/// Get all linked elements to this node
		/// </summary>
		public List<Element3D> Children { get; } = new List<Element3D>();

		public Node() : base() { }

		public Node(string name) : base(name) { }

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
