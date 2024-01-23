using CSMath;
using CSUtilities.Extensions;
using MeshIO.Entities;
using MeshIO.FBX.Connections;
using MeshIO.Shaders;
using System.Collections.Generic;

namespace MeshIO.FBX.Readers.Templates
{
	internal class FbxNodeTemplate : FbxObjectTemplate<Node>
	{
		public override string FbxObjectName { get { return FbxFileToken.Model; } }

		public FbxNodeTemplate(FbxNode node) : base(node, new Node())
		{
		}

		protected FbxNodeTemplate(Node root) : base(root)
		{
		}

		public override void Build(FbxFileBuilderBase builder)
		{
			base.Build(builder);

			this.processChildren(builder);
		}

		protected override void addProperties(Dictionary<string, FbxProperty> properties)
		{
			if(properties.Remove("Lcl Translation", out FbxProperty value))
			{
				this.Element.Transform.Translation = (XYZ)value.ToProperty().Value;
			}

			base.addProperties(properties);
		}

		protected void processChildren(FbxFileBuilderBase builder)
		{
			foreach (FbxConnection c in builder.GetChildren(this.TemplateId))
			{
				if (!builder.TryGetTemplate(c.ChildId, out IFbxObjectTemplate template))
				{
					builder.Notify($"[{this.Element.GetType().FullName}] child object not found {c.ChildId}", Core.NotificationType.Warning);
					continue;
				}

				this.addChild(template.GetElement());

				template.Build(builder);
			}
		}

		protected void addChild(Element3D element)
		{
			switch (element)
			{
				case Node node:
					this.Element.Nodes.Add(node);
					break;
				case Material mat:
					this.Element.Materials.Add(mat);
					break;
				case Entity entity:
					this.Element.Entities.Add(entity);
					break;
				default:
					break;
			}
		}
	}
}
