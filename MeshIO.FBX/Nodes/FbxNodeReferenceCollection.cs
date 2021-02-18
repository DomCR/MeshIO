using MeshIO.FBX.Attributes;
using MeshIO.FBX.IO;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MeshIO.FBX.Nodes
{
	public abstract class FbxNodeReferenceCollection<T> : FbxEmitter
		where T : FbxEmitter
	{
		protected List<T> m_children = new List<T>();
		//****************************************************************
		public FbxNodeReferenceCollection() { }
		public FbxNodeReferenceCollection(FbxNode node)
		{
			createByReflection(node);
		}
		//****************************************************************
		/// <summary>
		/// Add an item to the collection.
		/// </summary>
		/// <param name="item"></param>
		public virtual void Add(T item)
		{
			m_children.Add(item);
		}
		/// <summary>
		/// Clear the collection of any item.
		/// </summary>
		public void Clear()
		{
			m_children.Clear();
		}
		/// <inheritdoc/>
		public override FbxNode ToFbxNode()
		{
			FbxNode node = base.ToFbxNode();

			//Add the references in this node
			foreach (T item in m_children)
			{
				node.Nodes.Add(item.ToFbxNode());
			}

			return node;
		}
		//****************************************************************
		protected override void createByReflection(FbxNode node)
		{
			Dictionary<string, PropertyInfo> map = FbxNodeBuilder.CreateReferenceMap(GetType());

			//Set the values into this document
			foreach (FbxNode n in node.Nodes)
			{
				//Check if is a parameter of this object
				if (map.TryGetValue(n.Name, out PropertyInfo prop))
				{
					//Create an instance of the object using the node as parameter
					object o = Activator.CreateInstance(prop.PropertyType, n);
					//Set the value to this property
					prop.SetValue(this, o);
				}
				//Build a colleciton element
				else
				{
					//Build an element for this collection
					T child = buildChild(n);

					if (child != null)
						//Add the child to the collection
						m_children.Add(child);
					else
						//The node is not listed, added as a user node
						UserNodes.Add(n);
				}
			}
		}
		/// <summary>
		/// Method to build a child of the current collection using a <see cref="FbxNode"/>.
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		protected abstract T buildChild(FbxNode node);
	}
}
