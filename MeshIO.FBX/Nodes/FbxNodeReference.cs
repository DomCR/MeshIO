using MeshIO.FBX.Attributes;
using MeshIO.FBX.Exceptions;
using MeshIO.FBX.IO;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MeshIO.FBX.Nodes
{
	/* 
	 * [Class]: { }
	 */
	/// <summary>
	/// Represents a documented node in a fbx file.
	/// </summary>
	public abstract class FbxNodeReference : IFbxNodeReference, IFbxNamedNode
	{
		public abstract string ClassName { get; }
		/// <summary>
		/// Custom fbx nodes added by the user.
		/// </summary>
		/// <remarks>
		/// The addition of custom nodes may result into a file corruption.
		/// </remarks>
		public List<FbxNode> UserNodes { get; set; } = new List<FbxNode>();
		//****************************************************************
		public FbxNodeReference() { }
		/// <summary>
		/// Create a node reference based on a node.
		/// </summary>
		/// <param name="node">Version by default <see cref="FbxVersion.v7400"/> </param>
		public FbxNodeReference(FbxNode node)
		{
			createByReflection(node);
		}
		/// <summary>
		/// Create a node reference based on a node.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="version"></param>
		public FbxNodeReference(FbxNode node, FbxVersion version)
		{

		}
		//****************************************************************
		public virtual string GetClassName(FbxVersion version)
		{
			//TODO: This will allow the compatibility multiple fbx versions
			throw new NotImplementedException();
		}
		public virtual FbxNode ToFbxNode()
		{
			FbxNode node = new FbxNode(ClassName);

			node.Nodes.AddRange(createNodeChildren());
			node.Nodes.AddRange(UserNodes);

			return node;
		}
		public virtual FbxNode ToFbxNode(FbxVersion version)
		{
			throw new NotImplementedException();
		}
		//****************************************************************
		protected List<FbxNode> createNodeChildren()
		{
			List<FbxNode> children = new List<FbxNode>();

			//Setup the fbx children
			Type t = this.GetType();
			foreach (PropertyInfo prop in t.GetProperties())
			{
				FbxChildNodeAttribute att = prop.GetCustomAttribute<FbxChildNodeAttribute>();
				if (att == null)
					continue;

				object value = prop.GetValue(this);
				//Set the node by reference
				if (value is IFbxNodeReference reference)
				{
					FbxNode child = reference.ToFbxNode();

					if (child != null)
						children.Add(child);
				}
				//The node is a single value node
				else if (value != null && prop.PropertyType.IsPrimitive)
				{
					children.Add(new FbxNode(att.Name, value));
				}
			}

			return children;
		}
		protected virtual void createByReflection(FbxNode node)
		{
			Dictionary<string, PropertyInfo> map = FbxNodeBuilder.CreateReferenceMap(this.GetType());

			//Set the values into this document
			foreach (var n in node.Nodes)
			{
				if (map.TryGetValue(n.Name, out PropertyInfo prop))
				{
					if (prop.PropertyType.BaseType == typeof(FbxNodeReference))
					{
						//Create the node reference passing the node as parameter
						object value = Activator.CreateInstance(prop.PropertyType, n);
						prop.SetValue(this, value);
					}
					else
					{
						if (prop.PropertyType.IsEnum)
						{

						}
						else
						{
							//Primitive typed value, create the node by assigning the raw value
							object value = Convert.ChangeType(n.Value, prop.PropertyType);
							prop.SetValue(this, value);
						}
					}
				}
				else
				{
					//Add the node as a custom node
					this.UserNodes.Add(new FbxNode(n));
				}
			}
		}
	}
}
