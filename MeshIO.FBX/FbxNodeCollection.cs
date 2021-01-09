using System.Collections;
using System.Collections.Generic;

namespace MeshIO.FBX
{
	/// <summary>
	/// Base class for nodes and documents
	/// </summary>
	public abstract class FbxNodeCollection 
	{
		/// <summary>
		/// The list of child/nested nodes
		/// </summary>
		/// <remarks>
		/// A list with one or more null elements is treated differently than an empty list,
		/// and represented differently in all FBX output files.
		/// </remarks>
		public List<FbxNode> Nodes { get; set; } = new List<FbxNode>();
		/// <summary>
		/// Gets a named child node.
		/// </summary>
		/// <param name="name"></param>
		/// <remarks>
		/// This method is not useful for the multiple named nodes such as Objects, Connections...
		/// </remarks>
		/// <returns>The child node, or null</returns>
		public FbxNode this[string name]
		{
			get { return Nodes.Find(n => n != null && n.Name == name); }
			set
			{
				var child = Nodes.Find(n => n != null && n.Name == name);
				//Set the value at the existing node
				if (child != null)
				{
					Nodes[Nodes.IndexOf(child)] = value;
					child = value;
				}
				//Node not found, add the current node
				else
					Nodes.Add(value);
			}
		}
		/// <summary>
		/// Default constructor. Initializes an empty node collection.
		/// </summary>
		public FbxNodeCollection() { }
		//********************************************************************************************
		/// <summary>
		/// Gets a child node, using a '/' separated path
		/// </summary>
		/// <param name="path"></param>
		/// <returns>The child node, or null</returns>
		public FbxNode GetRelative(string path)
		{
			var tokens = path.Split('/');
			FbxNodeCollection n = this;
			foreach (var t in tokens)
			{
				if (t == "")
					continue;
				n = n[t];
				if (n == null)
					break;
			}
			return n as FbxNode;
		}
	}
}
