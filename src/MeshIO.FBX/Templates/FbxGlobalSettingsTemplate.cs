using MeshIO.FBX.Readers;
using MeshIO.FBX.Writers;
using System.Collections.Generic;

namespace MeshIO.FBX.Templates
{
	internal class FbxGlobalSettingsTemplate : IFbxObjectTemplate
	{
		public string Id { get; }

		public string Name { get; }

		public string FbxObjectName { get; } = FbxFileToken.GlobalSettings;

		public string FbxTypeName { get; }

		public List<FbxProperty> FbxProperties { get; } = new();

		public FbxGlobalSettingsTemplate()
		{
			FbxProperties.Add(new("UpAxis", "int", "Integer", PropertyFlags.None, 1));
			FbxProperties.Add(new("UpAxisSign", "int", "Integer", PropertyFlags.None, 1));
			FbxProperties.Add(new("FrontAxis", "int", "Integer", PropertyFlags.None, 2));
			FbxProperties.Add(new("FrontAxisSign", "int", "Integer", PropertyFlags.None, 1));
			FbxProperties.Add(new("CoordAxis", "int", "Integer", PropertyFlags.None, 0));
			FbxProperties.Add(new("CoordAxisSign", "int", "Integer", PropertyFlags.None, 1));
			FbxProperties.Add(new("OriginalUpAxis", "int", "Integer", PropertyFlags.None, 2));
			FbxProperties.Add(new("OriginalUpAxisSign", "int", "Integer", PropertyFlags.None, 1));
			FbxProperties.Add(new("UnitScaleFactor", "double", "Number", PropertyFlags.None, 1.0d));
			FbxProperties.Add(new("OriginalUnitScaleFactor", "double", "Number", PropertyFlags.None, 1.0d));
			FbxProperties.Add(new("AmbientColor", "ColorRGB", "Color", PropertyFlags.None, new Color()));
			FbxProperties.Add(new("DefaultCamera", "KString", string.Empty, PropertyFlags.None, "Producer Perspective"));
			FbxProperties.Add(new("TimeMode", "enum", string.Empty, PropertyFlags.None, 6));
			FbxProperties.Add(new("TimeProtocol", "enum", string.Empty, PropertyFlags.None, 2));
			FbxProperties.Add(new("SnapOnFrameMode", "enum", string.Empty, PropertyFlags.None, 0));
			FbxProperties.Add(new("TimeSpanStart", "KTime", "Time", PropertyFlags.None, 0));
			FbxProperties.Add(new("TimeSpanStop", "KTime", "Time", PropertyFlags.None, 153953860000));
			FbxProperties.Add(new("CustomFrameRate", "double", "Number", PropertyFlags.None, -1.0d));
			FbxProperties.Add(new("TimeMarker", "Compound", string.Empty, PropertyFlags.None, string.Empty));
			FbxProperties.Add(new("CurrentTimeMarker", "int", "Integer", PropertyFlags.None, -1));
		}

		public void ProcessChildren(FbxFileWriterBase fbxFileWriterBase)
		{
			throw new System.InvalidOperationException();
		}

		public void ApplyTemplate(FbxPropertyTemplate template)
		{
			throw new System.InvalidOperationException();
		}

		public Element3D GetElement()
		{
			throw new System.InvalidOperationException();
		}

		public void Build(FbxFileBuilderBase builder)
		{
			throw new System.InvalidOperationException();
		}

		public FbxNode ToFbxNode(FbxFileWriterBase writer)
		{
			throw new System.InvalidOperationException();
		}
	}
}
