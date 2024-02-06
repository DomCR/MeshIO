using MeshIO.FBX.Readers;
using MeshIO.FBX.Writers;
using System.Collections.Generic;

namespace MeshIO.FBX.Templates
{
	internal class FbxGlobalSettingsWriter : IFbxObjectTemplate
	{
		public string Id { get; }

		public string Name { get; }

		public string FbxObjectName { get; } = FbxFileToken.GlobalSettings;

		public string FbxTypeName { get; }

		public List<FbxProperty> FbxProperties { get; } = new();

		public FbxGlobalSettingsWriter()
		{
			FbxProperties.Add(new("UpAxis", 1));
			FbxProperties.Add(new("UpAxisSign", 1));
			FbxProperties.Add(new("FrontAxis", 2));
			FbxProperties.Add(new("FrontAxisSign", 1));
			FbxProperties.Add(new("CoordAxis", 0));
			FbxProperties.Add(new("CoordAxisSign", 1));
			FbxProperties.Add(new("OriginalUpAxis", -1));
			FbxProperties.Add(new("OriginalUpAxisSign", 1));
			FbxProperties.Add(new("UnitScaleFactor", (double)1));
			FbxProperties.Add(new("OriginalUnitScaleFactor", (double)1));
			FbxProperties.Add(new("AmbientColor", new Color()));
			FbxProperties.Add(new("DefaultCamera", "Producer Perspective"));
			FbxProperties.Add(new("TimeMode", "enum", string.Empty, PropertyFlags.None, 6));
			FbxProperties.Add(new("TimeProtocol", "enum", string.Empty, PropertyFlags.None, 2));
			FbxProperties.Add(new("SnapOnFrameMode", "enum", string.Empty, PropertyFlags.None, 0));
			FbxProperties.Add(new("TimeSpanStart", "KTime", "Time", PropertyFlags.None, 0));
			FbxProperties.Add(new("TimeSpanStop", "KTime", "Time", PropertyFlags.None, 153953860000));
			FbxProperties.Add(new("CustomFrameRate", (double)-1));
			FbxProperties.Add(new("TimeMarker", "Compound", string.Empty, PropertyFlags.None, string.Empty));
			FbxProperties.Add(new("CurrentTimeMarker", -1));
		}

		public void ProcessChildren(FbxFileWriterBase fbxFileWriterBase)
		{
			throw new System.InvalidOperationException();
		}

		public void ApplyTemplate(FbxPropertyTemplate template)
		{
			throw new System.NotImplementedException();
		}

		public Element3D GetElement()
		{
			throw new System.NotImplementedException();
		}

		public void Build(FbxFileBuilderBase builder)
		{
			throw new System.NotImplementedException();
		}

		public FbxNode ToFbxNode(FbxFileWriterBase writer)
		{
			throw new System.NotImplementedException();
		}
	}
}
