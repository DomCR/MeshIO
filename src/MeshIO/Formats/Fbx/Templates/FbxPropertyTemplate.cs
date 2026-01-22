using CSMath;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO.Formats.Fbx.Templates;

public class FbxPropertyTemplate
{
	public string ObjectTypeName { get; }

	public string Name { get; }

	public Dictionary<string, FbxProperty> Properties { get; } = new();

	public FbxPropertyTemplate() : this(string.Empty, string.Empty, []) { }

	public FbxPropertyTemplate(string objectTypeName, string name, Dictionary<string, FbxProperty> properties)
	{
		this.ObjectTypeName = objectTypeName;
		this.Name = name;
		this.Properties = properties;
	}

	public static FbxPropertyTemplate Create(string fbxObjectType)
	{
		switch (fbxObjectType)
		{
			case FbxFileToken.Model:
				return new FbxPropertyTemplate("Model", "FbxNode", getFbxNodeTemplate());
			case FbxFileToken.Geometry:
				return new FbxPropertyTemplate("Geometry", "FbxMesh", getFbxGeometryTemplate());
			case FbxFileToken.Material:
				return new FbxPropertyTemplate("Material", "FbxSurfaceMaterial", getFbxMaterialTemplate());
			default:
				throw new ArgumentException($"Unknown fbx ObjectType name {fbxObjectType}");
		}
	}

	private static Dictionary<string, FbxProperty> getFbxNodeTemplate()
	{
		return new List<FbxProperty>
		{
			new FbxProperty("QuaternionInterpolate", "enum", "", PropertyFlags.None, 0) ,
			new FbxProperty("RotationOffset", "Vector3D", "Vector", PropertyFlags.None,XYZ.Zero),
			new FbxProperty("RotationPivot", "Vector3D", "Vector", PropertyFlags.None,XYZ.Zero),
			new FbxProperty("ScalingOffset", "Vector3D", "Vector", PropertyFlags.None,XYZ.Zero),
			new FbxProperty("ScalingPivot", "Vector3D", "Vector", PropertyFlags.None,XYZ.Zero),
			new FbxProperty("TranslationActive", "bool", "", PropertyFlags.None,false),
			new FbxProperty("TranslationMin", "Vector3D", "Vector", PropertyFlags.None,XYZ.Zero),
			new FbxProperty("TranslationMax", "Vector3D", "Vector", PropertyFlags.None,XYZ.Zero),
			new FbxProperty("TranslationMinX", "bool", "", PropertyFlags.None,false),
			new FbxProperty("TranslationMinY", "bool", "", PropertyFlags.None,false),
			new FbxProperty("TranslationMinZ", "bool", "", PropertyFlags.None,false),
			new FbxProperty("TranslationMaxX", "bool", "", PropertyFlags.None,false),
			new FbxProperty("TranslationMaxY", "bool", "", PropertyFlags.None,false),
			new FbxProperty("TranslationMaxZ", "bool", "", PropertyFlags.None,false),
			new FbxProperty("RotationOrder", "enum", "", PropertyFlags.None,0),
			new FbxProperty("RotationSpaceForLimitOnly", "bool", "", PropertyFlags.None,false),
			new FbxProperty("RotationStiffnessX", "double", "Number", PropertyFlags.None,0.0d),
			new FbxProperty("RotationStiffnessY", "double", "Number", PropertyFlags.None,0.0d),
			new FbxProperty("RotationStiffnessZ", "double", "Number", PropertyFlags.None,0.0d),
			new FbxProperty("AxisLen", "double", "Number", PropertyFlags.None,10.0d),
			new FbxProperty("PreRotation", "Vector3D", "Vector", PropertyFlags.None,XYZ.Zero),
			new FbxProperty("PostRotation", "Vector3D", "Vector", PropertyFlags.None,XYZ.Zero),
			new FbxProperty("RotationActive", "bool", "", PropertyFlags.None,false),
			new FbxProperty("RotationMin", "Vector3D", "Vector", PropertyFlags.None,XYZ.Zero),
			new FbxProperty("RotationMax", "Vector3D", "Vector", PropertyFlags.None,XYZ.Zero),
			new FbxProperty("RotationMinX", "bool", "", PropertyFlags.None, false),
			new FbxProperty("RotationMinY", "bool", "", PropertyFlags.None, false),
			new FbxProperty("RotationMinZ", "bool", "", PropertyFlags.None, false),
			new FbxProperty("RotationMaxX", "bool", "", PropertyFlags.None, false),
			new FbxProperty("RotationMaxY", "bool", "", PropertyFlags.None, false),
			new FbxProperty("RotationMaxZ", "bool", "", PropertyFlags.None, false),
			new FbxProperty("InheritType", "enum", "", PropertyFlags.None, 0),
			new FbxProperty("ScalingActive", "bool", "", PropertyFlags.None, 0),
			new FbxProperty("ScalingMin", "Vector3D", "Vector", PropertyFlags.None,XYZ.Zero),
			new FbxProperty("ScalingMax", "Vector3D", "Vector", PropertyFlags.None,new XYZ(1,1,1)),
			new FbxProperty("ScalingMinX", "bool", "", PropertyFlags.None, false),
			new FbxProperty("ScalingMinY", "bool", "", PropertyFlags.None, false),
			new FbxProperty("ScalingMinZ", "bool", "", PropertyFlags.None, false),
			new FbxProperty("ScalingMaxX", "bool", "", PropertyFlags.None, false),
			new FbxProperty("ScalingMaxY", "bool", "", PropertyFlags.None, false),
			new FbxProperty("ScalingMaxZ", "bool", "", PropertyFlags.None, false),
			new FbxProperty("GeometricTranslation", "Vector3D", "Vector", PropertyFlags.None,XYZ.Zero),
			new FbxProperty("GeometricRotation", "Vector3D", "Vector", PropertyFlags.None,XYZ.Zero),
			new FbxProperty("GeometricScaling", "Vector3D", "Vector", PropertyFlags.None,new XYZ(1,1,1)),
			new FbxProperty("MinDampRangeX", "double", "Number", PropertyFlags.None,0.0d),
			new FbxProperty("MinDampRangeY", "double", "Number", PropertyFlags.None,0.0d),
			new FbxProperty("MinDampRangeZ", "double", "Number", PropertyFlags.None,0.0d),
			new FbxProperty("MaxDampRangeX", "double", "Number", PropertyFlags.None,0.0d),
			new FbxProperty("MaxDampRangeY", "double", "Number", PropertyFlags.None,0.0d),
			new FbxProperty("MaxDampRangeZ", "double", "Number", PropertyFlags.None,0.0d),
			new FbxProperty("MinDampStrengthX", "double", "Number", PropertyFlags.None,0.0d),
			new FbxProperty("MinDampStrengthY", "double", "Number", PropertyFlags.None,0.0d),
			new FbxProperty("MinDampStrengthZ", "double", "Number", PropertyFlags.None,0.0d),
			new FbxProperty("MaxDampStrengthX", "double", "Number", PropertyFlags.None,0.0d),
			new FbxProperty("MaxDampStrengthY", "double", "Number", PropertyFlags.None,0.0d),
			new FbxProperty("MaxDampStrengthZ", "double", "Number", PropertyFlags.None,0.0d),
			new FbxProperty("PreferedAngleX", "double", "Number", PropertyFlags.None,0.0d),
			new FbxProperty("PreferedAngleY", "double", "Number", PropertyFlags.None,0.0d),
			new FbxProperty("PreferedAngleZ", "double", "Number", PropertyFlags.None,0.0d),
			new FbxProperty("LookAtProperty", "object", "", PropertyFlags.None,null),
			new FbxProperty("UpVectorProperty", "object", "", PropertyFlags.None ,null),
			new FbxProperty("Show", "bool", "", PropertyFlags.None,true),
			new FbxProperty("NegativePercentShapeSupport", "bool", "", PropertyFlags.None, true),
			new FbxProperty("DefaultAttributeIndex", "int", "Integer", PropertyFlags.None, 0),
			new FbxProperty("Freeze", "bool", "", PropertyFlags.None,false),
			new FbxProperty("LODBox", "bool", "", PropertyFlags.None,false),
			new FbxProperty("Lcl Translation", "Lcl Translation", "", PropertyFlags.Animatable,XYZ.Zero),
			new FbxProperty("Lcl Rotation", "Lcl Rotation", "", PropertyFlags.Animatable,XYZ.Zero),
			new FbxProperty("Lcl Scaling", "Lcl Scaling", "", PropertyFlags.Animatable,new XYZ(1,1,1)),
			new FbxProperty("Visibility", "Visibility", "", PropertyFlags.Animatable,1),
			new FbxProperty("Visibility Inheritance", "Visibility Inheritance", "", PropertyFlags.None,1),
		}.ToDictionary(p => p.Name, p => p);
	}

	private static Dictionary<string, FbxProperty> getFbxGeometryTemplate()
	{
		return new List<FbxProperty>
		{
			new FbxProperty("Color", "ColorRGB", "Color", PropertyFlags.None, new Color(204,204,204)),
			new FbxProperty("BBoxMin", "Vector3D", "Vector", PropertyFlags.None, XYZ.Zero),
			new FbxProperty("BBoxMax", "Vector3D", "Vector", PropertyFlags.None, XYZ.Zero),
			new FbxProperty("Primary Visibility", "bool", "", PropertyFlags.None, true),
			new FbxProperty("Casts Shadows", "bool", "", PropertyFlags.None, true),
			new FbxProperty("Receive Shadows", "bool", "", PropertyFlags.None, true),
		}.ToDictionary(p => p.Name, p => p);
	}

	private static Dictionary<string, FbxProperty> getFbxMaterialTemplate()
	{
		return new List<FbxProperty>
		{
			new FbxProperty("ShadingModel", "KString", "", PropertyFlags.None, "Unknown"),
			new FbxProperty("MultiLayer", "bool", "", PropertyFlags.None, false),
		}.ToDictionary(p => p.Name, p => p);
	}

}