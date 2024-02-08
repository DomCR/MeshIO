using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO.FBX
{
	/// <summary>
	/// Base class for nodes and documents
	/// </summary>
	public abstract class FbxNodeCollection : IEnumerable<FbxNode>
	{
		/// <summary>
		/// The list of child/nested nodes
		/// </summary>
		/// <remarks>
		/// A list with one or more null elements is treated differently than an empty list,
		/// and represented differently in all FBX output files.
		/// </remarks>
		public List<FbxNode> Nodes { get; } = new List<FbxNode>();

		/// <summary>
		/// Gets a named child node
		/// </summary>
		/// <param name="name"></param>
		/// <returns>The child node, or null</returns>
		public FbxNode this[string name] { get { return this.Nodes.Find(n => n != null && n.Name == name); } }

		/// <summary>
		/// Add a note into the collection
		/// </summary>
		/// <param name="name"></param>
		/// <param name="args"></param>
		/// <returns>the added node</returns>
		public FbxNode Add(string name, params object[] args)
		{
			FbxNode n = new FbxNode(name, args);
			this.Nodes.Add(n);
			return n;
		}

		/// <summary>
		/// Gets a child node, using a '/' separated path
		/// </summary>
		/// <param name="path"></param>
		/// <returns>The child node, or null</returns>
		public FbxNode GetRelative(string path)
		{
			var tokens = path.Split('/');
			FbxNodeCollection current = this as FbxNodeCollection;
			foreach (string t in tokens)
			{
				if (t == "")
					continue;
				current = current[t];
				if (current == null)
					break;
			}
			return current as FbxNode;
		}

		/// <summary>
		/// Checks if the name of the node is repeated
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool MultipleNodes(string name)
		{
			return this.Nodes.Where(n => n.Name == name).Count() > 1;
		}

		public IEnumerable<FbxNode> GetNodes(string name)
		{
			return this.Nodes.Where(n => n.Name == name);
		}

		/// <summary>
		/// Gets the first named node if exists
		/// </summary>
		/// <param name="name"></param>
		/// <param name="node"></param>
		/// <returns></returns>
		public bool TryGetNode(string name, out FbxNode node)
		{
			node = this[name];
			return node != null;
		}

		/// <inheritdoc/>
		public IEnumerator<FbxNode> GetEnumerator()
		{
			return Nodes.GetEnumerator();
		}

		/// <inheritdoc/>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return Nodes.GetEnumerator();
		}
	}
}
