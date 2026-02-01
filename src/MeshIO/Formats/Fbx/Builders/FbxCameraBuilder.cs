using CSMath;
using CSUtilities.Extensions;
using MeshIO.Entities;
using MeshIO.Formats.Fbx.Readers;
using System;

namespace MeshIO.Formats.Fbx.Builders;

internal class FbxCameraBuilder : FbxObjectBuilder<Camera>
{
	public override string FbxObjectName { get { return FbxFileToken.NodeAttribute; } }

	public override string FbxTypeName { get { return FbxFileToken.Camera; } }

	public FbxCameraBuilder(FbxNode node) : base(node, new Camera())
	{
	}

	public override void Build(FbxFileBuilderBase builder)
	{
		base.Build(builder);
	}

	protected override bool setValue(FbxFileBuilderBase builder, FbxNode node)
	{
		switch (node.Name)
		{
			case FbxFileToken.Position:
				this._element.Position = this.nodeToXYZ(node);
				return true;
			case FbxFileToken.Up:
				this._element.UpVector = this.nodeToXYZ(node).Normalize();
				return true;
			case FbxFileToken.LookAt:
				this._element.LookAt = this.nodeToXYZ(node).Normalize();
				return true;
			default:
				return base.setValue(builder, node);
		}
	}
}
