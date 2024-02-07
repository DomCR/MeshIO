#if NETFRAMEWORK
using CSUtilities.Extensions;
#endif
using CSMath;
using MeshIO.Entities;
using MeshIO.FBX.Connections;
using MeshIO.FBX.Readers;
using MeshIO.FBX.Writers;
using MeshIO.Shaders;
using System.Collections.Generic;
using System.Reflection;

namespace MeshIO.FBX.Templates
{
	internal class FbxNodeTemplate : FbxObjectTemplate<Node>
	{
		public override string FbxObjectName { get { return FbxFileToken.Model; } }

		public override string FbxTypeName { get { return FbxFileToken.Mesh; } }

		public FbxNodeTemplate(FbxNode node) : base(node, new Node())
		{
		}

		public FbxNodeTemplate(Node root) : base(root)
		{
		}

		public override void Build(FbxFileBuilderBase builder)
		{
			base.Build(builder);

			processChildren(builder);
		}

		protected override void addObjectBody(FbxNode node, FbxFileWriterBase writer)
		{
			node.Add(FbxFileToken.Version, 232);

			base.addObjectBody(node, writer);

			node.Add(FbxFileToken.Shading, 'T');
			node.Add(FbxFileToken.CullingOff, "CullingOff");
		}

		public override void ProcessChildren(FbxFileWriterBase fwriter)
		{
			base.ProcessChildren(fwriter);

			foreach (Node node in this._element.Nodes)
			{
				fwriter.CreateConnection(node, this);
			}

			foreach (Shaders.Material mat in this._element.Materials)
			{
				fwriter.CreateConnection(mat, this);
			}

			foreach (Entities.Entity entity in this._element.Entities)
			{
				fwriter.CreateConnection(entity, this);
			}
		}

		protected override void addProperties(Dictionary<string, FbxProperty> properties)
		{
			if (properties.Remove("Lcl Translation", out FbxProperty translation))
			{
				_element.Transform.Translation = (XYZ)translation.ToProperty().Value;
			}

			if (properties.Remove("Lcl Rotation", out FbxProperty rotation))
			{
				_element.Transform.Translation = (XYZ)rotation.ToProperty().Value;
			}

			if (properties.Remove("Lcl Scaling", out FbxProperty scaling))
			{
				_element.Transform.Translation = (XYZ)scaling.ToProperty().Value;
			}

			base.addProperties(properties);
		}

		protected void processChildren(FbxFileBuilderBase builder)
		{
			foreach (FbxConnection c in builder.GetChildren(Id))
			{
				if (!builder.TryGetTemplate(c.ChildId, out IFbxObjectTemplate template))
				{
					builder.Notify($"[{_element.GetType().FullName}] child object not found {c.ChildId}", Core.NotificationType.Warning);
					continue;
				}

				addChild(template.GetElement());

				template.Build(builder);
			}
		}

		protected void addChild(Element3D element)
		{
			switch (element)
			{
				case Node node:
					_element.Nodes.Add(node);
					break;
				case Material mat:
					_element.Materials.Add(mat);
					break;
				case Entity entity:
					_element.Entities.Add(entity);
					break;
				default:
					break;
			}
		}
	}
}
