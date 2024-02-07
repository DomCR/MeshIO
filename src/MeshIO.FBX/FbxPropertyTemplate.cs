using CSMath;
using MeshIO.Entities.Geometries;
using MeshIO.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO.FBX
{
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

		public static FbxPropertyTemplate Create<T>(T element)
			where T : SceneElement
		{
			switch (element)
			{
				case Node:
					return new FbxPropertyTemplate("Model", "FbxNode", getFbxNodeTemplate());
				case Geometry:
					return new FbxPropertyTemplate("Geometry", "FbxMesh", getFbxGeometryTemplate());
				case Material:
					return new FbxPropertyTemplate("Material", "FbxSurfaceMaterial", getFbxMaterialTemplate());
				default:
					throw new ArgumentException();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="fbxProperty"></param>
		/// <returns>True if the property has been updated</returns>
		public bool GetUpdated(string name, object value, out FbxProperty fbxProperty)
		{
			if (this.Properties.TryGetValue(name, out FbxProperty existing)
				&& !value.Equals(existing.Value))
			{
				fbxProperty = new FbxProperty(existing, value);
				return true;
			}
			else
			{
				fbxProperty = null;
				return false;
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
				new FbxProperty("TranslationActive", "bool", "", PropertyFlags.None,0),
				new FbxProperty("TranslationMin", "Vector3D", "Vector", PropertyFlags.None,XYZ.Zero),
				new FbxProperty("TranslationMax", "Vector3D", "Vector", PropertyFlags.None,XYZ.Zero),
				new FbxProperty("TranslationMinX", "bool", "", PropertyFlags.None,0),
				new FbxProperty("TranslationMinY", "bool", "", PropertyFlags.None,0),
				new FbxProperty("TranslationMinZ", "bool", "", PropertyFlags.None,0),
				new FbxProperty("TranslationMaxX", "bool", "", PropertyFlags.None,0),
				new FbxProperty("TranslationMaxY", "bool", "", PropertyFlags.None,0),
				new FbxProperty("TranslationMaxZ", "bool", "", PropertyFlags.None,0),
				new FbxProperty("RotationOrder", "enum", "", PropertyFlags.None,0),
				new FbxProperty("RotationSpaceForLimitOnly", "bool", "", PropertyFlags.None,0),
				new FbxProperty("RotationStiffnessX", "double", "Number", PropertyFlags.None,0.0d),
				new FbxProperty("RotationStiffnessY", "double", "Number", PropertyFlags.None,0.0d),
				new FbxProperty("RotationStiffnessZ", "double", "Number", PropertyFlags.None,0.0d),
				new FbxProperty("AxisLen", "double", "Number", PropertyFlags.None,10),
				new FbxProperty("PreRotation", "Vector3D", "Vector", PropertyFlags.None,XYZ.Zero),
				new FbxProperty("PostRotation", "Vector3D", "Vector", PropertyFlags.None,XYZ.Zero),
				new FbxProperty("RotationActive", "bool", "", PropertyFlags.None,0),
				new FbxProperty("RotationMin", "Vector3D", "Vector", PropertyFlags.None,XYZ.Zero),
				new FbxProperty("RotationMax", "Vector3D", "Vector", PropertyFlags.None,XYZ.Zero),
				new FbxProperty("RotationMinX", "bool", "", PropertyFlags.None,0),
				new FbxProperty("RotationMinY", "bool", "", PropertyFlags.None,0),
				new FbxProperty("RotationMinZ", "bool", "", PropertyFlags.None,0),
				new FbxProperty("RotationMaxX", "bool", "", PropertyFlags.None,0),
				new FbxProperty("RotationMaxY", "bool", "", PropertyFlags.None,0),
				new FbxProperty("RotationMaxZ", "bool", "", PropertyFlags.None,0),
				new FbxProperty("InheritType", "enum", "", PropertyFlags.None,0),
				new FbxProperty("ScalingActive", "bool", "", PropertyFlags.None,0),
				new FbxProperty("ScalingMin", "Vector3D", "Vector", PropertyFlags.None,XYZ.Zero),
				new FbxProperty("ScalingMax", "Vector3D", "Vector", PropertyFlags.None,new XYZ(1,1,1)),
				new FbxProperty("ScalingMinX", "bool", "", PropertyFlags.None,0),
				new FbxProperty("ScalingMinY", "bool", "", PropertyFlags.None,0),
				new FbxProperty("ScalingMinZ", "bool", "", PropertyFlags.None,0),
				new FbxProperty("ScalingMaxX", "bool", "", PropertyFlags.None,0),
				new FbxProperty("ScalingMaxY", "bool", "", PropertyFlags.None,0),
				new FbxProperty("ScalingMaxZ", "bool", "", PropertyFlags.None,0),
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
				new FbxProperty("Show", "bool", "", PropertyFlags.None,1),
				new FbxProperty("NegativePercentShapeSupport", "bool", "", PropertyFlags.None,1),
				new FbxProperty("DefaultAttributeIndex", "int", "Integer", PropertyFlags.None, 0),
				new FbxProperty("Freeze", "bool", "", PropertyFlags.None,0),
				new FbxProperty("LODBox", "bool", "", PropertyFlags.None,0),
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
}