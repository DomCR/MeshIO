#if NETFRAMEWORK
using CSUtilities.Extensions;
#endif

using CSMath;
using MeshIO.Entities;
using MeshIO.Formats.Fbx.Connections;
using MeshIO.Formats.Fbx.Readers;
using MeshIO.Materials;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO.Formats.Fbx.Builders;

internal class FbxNodeBuilder : FbxObjectBuilder<Node>
{
	public override string FbxObjectName { get { return FbxFileToken.Model; } }

	public override string FbxTypeName { get; }

	public FbxNodeBuilder(FbxNode node) : base(node, new Node())
	{
		this.FbxTypeName = node.Properties.Last().ToString();
	}

	public FbxNodeBuilder(Node root) : base(root)
	{
	}

	public override void Build(FbxFileBuilderBase builder)
	{
		base.Build(builder);

		if (builder.Is6000Fbx
			&& this.FbxNode.TryGetNode("NodeAttributeName", out var nameNode)
			&& nameNode.Value.ToString().StartsWith("Geometry::"))
		{
			FbxMeshBuilder mesh = new FbxMeshBuilder(this.FbxNode);
			addChild(mesh.GetElement());
			mesh.Build(builder);
		}

		buildChildren(builder);
	}

	protected override bool setValue(FbxFileBuilderBase builder, FbxNode node)
	{
		switch (node.Name)
		{
			case FbxFileToken.Layer when builder.Is6000Fbx:
			case FbxFileToken.LayerElementNormal when builder.Is6000Fbx:
			case FbxFileToken.LayerElementBinormal when builder.Is6000Fbx:
			case FbxFileToken.LayerElementTangent when builder.Is6000Fbx:
			case FbxFileToken.LayerElementMaterial when builder.Is6000Fbx:
			case FbxFileToken.LayerElementUV when builder.Is6000Fbx:
			case FbxFileToken.LayerElementSmoothing when builder.Is6000Fbx:
			case FbxFileToken.Vertices when builder.Is6000Fbx:
			case FbxFileToken.Edges when builder.Is6000Fbx:
			case FbxFileToken.PolygonVertexIndex when builder.Is6000Fbx:
				return true;
			default:
				return base.setValue(builder, node);
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

	protected override void buildProperties(Dictionary<string, FbxProperty> properties)
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

		base.buildProperties(properties);
	}

	protected void buildChildren(FbxFileBuilderBase builder)
	{
		foreach (FbxConnection c in builder.GetChildren(Id))
		{
			if (!builder.TryGetTemplate(c.ChildId, out IFbxObjectBuilder template))
			{
				builder.Notify($"[{_element.GetType().FullName}] child object not found {c.ChildId}", NotificationType.Warning);
				continue;
			}

			addChild(template.GetElement());

			template.Build(builder);
		}
	}
}