#if NETFRAMEWORK
using CSUtilities.Extensions;
#endif

using CSMath;
using MeshIO.Entities;
using MeshIO.Formats.Fbx.Connections;
using MeshIO.Formats.Fbx.Readers;
using MeshIO.Formats.Fbx.Writers;
using MeshIO.Shaders;
using System.Collections.Generic;

namespace MeshIO.Formats.Fbx.Builders;

internal class FbxNodeBuilder : FbxObjectBuilder<Node>
{
	public override string FbxObjectName { get { return FbxFileToken.Model; } }

	public override string FbxTypeName { get { return FbxFileToken.Mesh; } }

	public FbxNodeBuilder(FbxNode node) : base(node, new Node())
	{
	}

	public FbxNodeBuilder(Node root) : base(root)
	{
	}

	public override void Build(FbxFileBuilderBase builder)
	{
		base.Build(builder);

		if (builder.Version < FbxVersion.v7000 
			&& this.FbxNode.TryGetNode("NodeAttributeName", out var nameNode)
			&& nameNode.Value.ToString().StartsWith("Geometry::"))
		{
			FbxMeshBuilder mesh = new FbxMeshBuilder(this.FbxNode);
			addChild(mesh.GetElement());
			mesh.Build(builder);
		}

		processChildren(builder);
	}

	public override void ProcessChildren(FbxFileWriterBase writer)
	{
		base.ProcessChildren(writer);

		foreach (Node node in this._element.Nodes)
		{
			writer.CreateConnection(node, this);
		}

		foreach (Material mat in this._element.Materials)
		{
			writer.CreateConnection(mat, this);
		}

		foreach (Entity entity in this._element.Entities)
		{
			writer.CreateConnection(entity, this);
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

	protected override void addObjectBody(FbxNode node, FbxFileWriterBase writer)
	{
		node.Add(FbxFileToken.Version, 232);

		base.addObjectBody(node, writer);

		node.Add(FbxFileToken.Shading, 'T');
		node.Add(FbxFileToken.CullingOff, "CullingOff");
	}

	protected override void processProperties(Dictionary<string, FbxProperty> properties)
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

		base.processProperties(properties);
	}

	protected void processChildren(FbxFileBuilderBase builder)
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