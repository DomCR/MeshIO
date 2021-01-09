using MeshIO.FBX.Attributes;
using MeshIO.FBX.Nodes;
using MeshIO.FBX.Nodes.Connections;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MeshIO.FBX
{
	/// <summary>
	/// Fbx document.
	/// </summary>
	/// <remarks>
	/// Based on <see cref="FbxVersion.v7400"/>
	/// </remarks>
	public class FbxRootDocument
	{
		[FbxChildNode("FBXHeaderExtension")]
		public FbxHeader Header { get; set; }
		[FbxChildNode("GlobalSettings")]
		public FbxGlobalSettings GlobalSettings { get; set; }
		[FbxChildNode("Documents")]
		public FbxDocumentCollection Documents { get; set; }
		[FbxChildNode("References")]
		public FbxReferenceCollection References { get; set; }
		[FbxChildNode("Definitions")]
		public FbxDefinitions Definitions { get; set; }
		[FbxChildNode("Objects")]
		public FbxObjectCollection Objects { get; set; }
		[FbxChildNode("Connections")]
		public FbxConnectionColletion Connections { get; set; }
		/// <summary>
		/// Custom fbx nodes added by the user.
		/// </summary>
		/// <remarks>
		/// The addition of custom nodes may result into a file corruption.
		/// </remarks>
		public List<FbxNode> UserNodes { get; set; } = new List<FbxNode>();
		//****************************************************************
		/// <summary>
		/// Default constructor.
		/// </summary>
		public FbxRootDocument() { }
		/// <summary>
		/// Create a root document by referencing a fbx root node.
		/// </summary>
		/// <param name="root"></param>
		/// <param name="useReflection">test purpose (to delete).</param>
		public FbxRootDocument(FbxRoot root, bool useReflection = false)
		{
			if (useReflection)
				createByReflection(root);
			else
				createByAssignation(root);
		}
		//****************************************************************
		/// <summary>
		/// Creates a fbx root node.
		/// </summary>
		/// <remarks>The default version of the node is <see cref="FbxVersion.v7400"/></remarks>
		/// <returns></returns>
		public FbxRoot CreateRootNode()
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// Creates a fbx root node.
		/// </summary>
		public FbxRoot CreateRootNode(FbxVersion version)
		{
			throw new NotImplementedException();
		}
		public List<GElement> ExtractElements()
		{
			throw new NotImplementedException();
		}
		//****************************************************************
		private void createByAssignation(FbxRoot root)
		{
			foreach (FbxNode node in root.Nodes)
			{
				switch (node.Name)
				{
					case "FBXHeaderExtension":
						Header = new FbxHeader(node);
						break;
					case "GlobalSettings":
						GlobalSettings = new FbxGlobalSettings(node);
						break;
					case "Documents":
						Documents = new FbxDocumentCollection(node);
						break;
					case "References":
						References = new FbxReferenceCollection(node);
						break;
					case "Definitions":
						Definitions = new FbxDefinitions(node);
						break;
					case "Objects":
						Objects = new FbxObjectCollection(node);
						break;
					case "Connections":
						Connections = new FbxConnectionColletion(node);
						break;
					default:
						UserNodes.Add(node);
						break;
				}
			}
		}
		private void createByReflection(FbxRoot root)
		{
			Dictionary<string, PropertyInfo> map = new Dictionary<string, PropertyInfo>();

			//Setup the fbx children
			Type t = this.GetType();
			foreach (PropertyInfo prop in t.GetProperties())
			{
				FbxChildNodeAttribute att = prop.GetCustomAttribute<FbxChildNodeAttribute>();
				if (att == null)
					continue;

				map.Add(att.Name, prop);
			}

			//Set the values into this document
			foreach (var n in root.Nodes)
			{
				if (map.TryGetValue(n.Name, out PropertyInfo prop))
				{
					object o = Activator.CreateInstance(prop.PropertyType, n);
					prop.SetValue(this, o);
				}
				else
				{
					this.UserNodes.Add(new FbxNode(n));
				}
			}
		}
	}
}
