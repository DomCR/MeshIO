﻿using MeshIO.Entities;
using MeshIO.Shaders;
using System;
using System.Collections.Generic;

namespace MeshIO
{
	public class Node : SceneElement
	{
		[Obsolete]
		public bool Shading { get; set; } = true;

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
		/// Get all linked elements to this node
		/// </summary>
		[Obsolete("Replace for the different collections, ")]
		public List<Element3D> Children { get; } = new();

		public List<Node> Nodes { get; } = new();

		public List<Material> Materials { get; } = new();

		public List<Entity> Entities { get; } = new();

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
