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

		this.assignValue("Position", (n) =>
		{
			this._element.Position = new CSMath.XYZ(
				n.GetProperty<double>(0),
				n.GetProperty<double>(1),
				n.GetProperty<double>(2)
				);
		});

		this.assignValue("Up", (n) =>
		{
			this._element.UpVector = new CSMath.XYZ(
				n.GetProperty<double>(0),
				n.GetProperty<double>(1),
				n.GetProperty<double>(2)
				).Normalize();
		});

		this.assignValue("LookAt", (n) =>
		{
			this._element.LookAt = new CSMath.XYZ(
				n.GetProperty<double>(0),
				n.GetProperty<double>(1),
				n.GetProperty<double>(2)
				).Normalize();
		});
	}

	protected void assignValue(string name, Action<FbxNode> assign)
	{
		if (this.FbxNode.TryGetNode(name, out FbxNode node))
		{
			assign.Invoke(node);
		}
	}
}
