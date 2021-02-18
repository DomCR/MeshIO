using MeshIO.FBX.Attributes;
using MeshIO.FBX.Nodes;
using MeshIO.FBX.Nodes.Connections;
using MeshIO.FBX.Nodes.Objects;
using MeshIO.FBX.Nodes.Objects.NodeAttributes;
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
		public FbxHeader Header { get; set; } = new FbxHeader();
		[FbxChildNode("GlobalSettings")]
		public FbxGlobalSettings GlobalSettings { get; set; } = new FbxGlobalSettings();
		[FbxChildNode("Documents")]
		public FbxDocumentCollection Documents { get; set; } = new FbxDocumentCollection();
		[FbxChildNode("References")]
		public FbxReferenceCollection References { get; set; } = new FbxReferenceCollection();
		[FbxChildNode("Definitions")]
		public FbxDefinitions Definitions { get; set; } = new FbxDefinitions();
		[FbxChildNode("Objects")]
		public FbxObjectCollection Objects { get; set; } = new FbxObjectCollection();
		[FbxChildNode("Connections")]
		public FbxConnectionColletion Connections { get; set; } = new FbxConnectionColletion();
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
		public FbxRootDocument()
		{
			//Add the default document
			Documents.Add(new FbxDocumentInfo { RootNode = 0 });

			//Add the global settings definition
			Definitions.Add("GlobalSettings");
		}
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
			FbxRoot root = new FbxRoot();

			root.Nodes.Add(Header.ToFbxNode());
			root.Nodes.Add(GlobalSettings.ToFbxNode());
			root.Nodes.Add(Documents.ToFbxNode());
			root.Nodes.Add(References.ToFbxNode());
			root.Nodes.Add(Definitions.ToFbxNode());
			root.Nodes.Add(Objects.ToFbxNode());
			root.Nodes.Add(Connections.ToFbxNode());

			return root;
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
		public void AddElement(GElement element)
		{
			AddElement(element, 0);
		}
		public void AddElement(GElement element, ulong containerId)
		{
			FbxModel model = new FbxModel(element);
			Objects.Add(model);
			Connections.Add(new FbxConnection(model.Id, containerId));
			Definitions.Add(model.ClassName);

			foreach (Geometries.Mesh g in element.Geometries)
			{
				FbxMesh fmesh = new FbxMesh(g);

				Objects.Add(fmesh);
				Connections.Add(new FbxConnection(fmesh.Id, model.Id));
				Definitions.Add(fmesh.ClassName);
			}

			foreach (Material m in element.GetMaterials())
			{
				FbxMaterial fmaterial = new FbxMaterial(m);

				Objects.Add(fmaterial);
				Connections.Add(new FbxConnection(fmaterial.Id, model.Id));
				Definitions.Add(fmaterial.ClassName);
			}

			foreach (GElement s in element.Subelements)
			{
				AddElement(s, model.Id);
			}
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
