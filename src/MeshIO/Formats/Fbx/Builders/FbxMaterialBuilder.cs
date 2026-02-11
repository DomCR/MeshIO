using MeshIO.Formats.Fbx.Readers;
using MeshIO.Shaders;
using System.Collections.Generic;
using CSUtilities.Extensions;

namespace MeshIO.Formats.Fbx.Builders;

internal abstract class FbxMaterialBuilder<T> : FbxObjectBuilder<T>
	where T : Material
{
	public override string FbxObjectName { get { return string.Empty; } }

	public override string FbxTypeName { get { return FbxFileToken.Material; } }

	public FbxMaterialBuilder(FbxNode node, T material) : base(node, material)
	{
	}

	protected override bool setValue(FbxFileBuilderBase builder, FbxNode node)
	{
		switch (node.Name)
		{
			case FbxFileToken.ShadingModel:
			case FbxFileToken.MultiLayer:
				return true;
			default:
				return base.setValue(builder, node);
		}
	}

	protected override void buildProperties(Dictionary<string, FbxProperty> properties)
	{
		if (properties.Remove("EmissiveColor", out FbxProperty emissiveColor))
		{
			_element.EmissiveColor = emissiveColor.ToProperty().GetValue<Color>();
		}

		if (properties.Remove("EmissiveFactor", out FbxProperty emissiveFactor))
		{
			_element.EmissiveFactor = emissiveFactor.ToProperty().GetValue<double>();
		}

		if (properties.Remove("AmbientColor", out FbxProperty AmbientColor))
		{
			_element.AmbientColor = emissiveFactor.ToProperty().GetValue<Color>();
		}

		if (properties.Remove("AmbientFactor", out FbxProperty ambientFactor))
		{
			_element.AmbientFactor = ambientFactor.ToProperty().GetValue<double>();
		}

		base.buildProperties(properties);
	}
}

internal class FbxShaderMaterialBuilder : FbxMaterialBuilder<ShaderMaterial>
{
	public FbxShaderMaterialBuilder(FbxNode node) : base(node, new ShaderMaterial())
	{
	}
}

internal class FbxPhongMaterialBuilder : FbxMaterialBuilder<PhongMaterial>
{
	public FbxPhongMaterialBuilder(FbxNode node) : base(node, new PhongMaterial())
	{
	}
}