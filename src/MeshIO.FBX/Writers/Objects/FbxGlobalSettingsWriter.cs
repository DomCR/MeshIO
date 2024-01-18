using System.Collections.Generic;

namespace MeshIO.FBX.Writers.Objects
{
	internal class FbxGlobalSettingsWriter : IFbxObjectWriter
	{
		public ulong Id { get; }

		public string Name { get; }

		public string FbxObjectName { get; } = FbxFileToken.GlobalSettings;

		public string FbxTypeName { get; }

		public List<FbxProperty> FbxProperties { get; } = new();

		public FbxGlobalSettingsWriter()
		{
			FbxProperties.Add(new("UpAxis", (int)1));
			FbxProperties.Add(new("UpAxisSign", (int)1));
			FbxProperties.Add(new("FrontAxis", (int)2));
			FbxProperties.Add(new("FrontAxisSign", (int)1));
			FbxProperties.Add(new("CoordAxis", (int)0));
			FbxProperties.Add(new("CoordAxisSign", (int)1));
			FbxProperties.Add(new("OriginalUpAxis", (int)-1));
			FbxProperties.Add(new("OriginalUpAxisSign", (int)1));
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
			FbxProperties.Add(new("CurrentTimeMarker", (int)-1));
		}

		public void ProcessChildren(FbxFileWriterBase fbxFileWriterBase)
		{
			throw new System.InvalidOperationException();
		}

		public void Write(FbxFileWriterBase fbxFileWriterBase, IFbxStreamWriter writer)
		{
			throw new System.InvalidOperationException();
		}

		public void ApplyTemplate(FbxPropertyTemplate template)
		{
			throw new System.NotImplementedException();
		}
	}
}
