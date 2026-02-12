using CSMath;
using CSUtilities.Extensions;
using MeshIO.Entities;
using MeshIO.Formats.Fbx.Readers;
using System;
using System.Collections.Generic;

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

	protected override void buildProperties(Dictionary<string, FbxProperty> properties)
	{
		if (properties.Remove("NearPlane", out FbxProperty nearPlane))
		{
			_element.NearPlane = (double)nearPlane.ToProperty().Value;
		}

		if (properties.Remove("FarPlane", out FbxProperty farPlane))
		{
			_element.FarPlane = (double)farPlane.ToProperty().Value;
		}

		if (properties.Remove("FieldOfView", out FbxProperty fieldOfView))
		{
			_element.FieldOfView = (double)fieldOfView.ToProperty().Value;
		}

		if (properties.Remove("OrthoZoom", out FbxProperty orthoZoom))
		{
			_element.OrthographicZoom = new XY((double)orthoZoom.ToProperty().Value);
		}

		base.buildProperties(properties);
	}

	protected override bool setValue(FbxFileBuilderBase builder, FbxNode node)
	{
		switch (node.Name)
		{
			//Ignore
			case "ShowInfoOnMoving":
			case "ShowAudio":
			case "AudioColor":
				return true;
			case FbxFileToken.CameraOrthoZoom:
				_element.OrthographicZoom = new XY(node.GetValue<double>());
				return true;
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