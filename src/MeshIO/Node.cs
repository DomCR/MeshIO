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

		public Transform Transform { get; internal set; } = new Transform();

		public Element3D Parent { get; }

		public List<Element3D> Children { get; } = new List<Element3D>();

		public Node() : base() { }

		public Node(string name) : base(name) { }
	}
}
