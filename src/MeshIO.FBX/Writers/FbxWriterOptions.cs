using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.FBX.Writers
{
	public class FbxWriterOptions
	{
		public GlobalSettings GlobalSettings { get; set; }

		public FbxWriterOptions(FbxVersion version)
		{

		}
	}

	public class GlobalSettings
	{
		public int Version { get; }

		public int UpAxis
		{
			get
			{
				return getPropertyValue<int>();
			}
			set
			{
				setPropertyValue(value);
			}
		}

		private PropertyCollection _properties = new PropertyCollection(null);

		public GlobalSettings(FbxVersion version)
		{
			switch (version)
			{
				case FbxVersion.v2000:
				case FbxVersion.v2001:
				case FbxVersion.v3000:
				case FbxVersion.v3001:
				case FbxVersion.v4000:
				case FbxVersion.v4001:
				case FbxVersion.v4050:
				case FbxVersion.v5000:
				case FbxVersion.v5800:
				case FbxVersion.v6000:
				case FbxVersion.v6100:
				case FbxVersion.v7000:
				case FbxVersion.v7100:
				case FbxVersion.v7200:
				case FbxVersion.v7300:
				case FbxVersion.v7400:
				case FbxVersion.v7500:
				case FbxVersion.v7600:
				case FbxVersion.v7700:
					Version = 1000;
					break;
				default:
					break;
			}

			_properties.Add(new FbxProperty<int>("UpAxis", null, 1, "int", "Integer", PropertyFlags.None));
			_properties.Add(new FbxProperty<int>("UpAxisSign", null, 1, "int", "Integer", PropertyFlags.None));
			_properties.Add(new FbxProperty<int>("FrontAxis", null, 2, "int", "Integer", PropertyFlags.None));
			_properties.Add(new FbxProperty<int>("FrontAxisSign", null, 1, "int", "Integer", PropertyFlags.None));
			_properties.Add(new FbxProperty<int>("CoordAxis", null, 0, "int", "Integer", PropertyFlags.None));
			_properties.Add(new FbxProperty<int>("CoordAxisSign", null, 1, "int", "Integer", PropertyFlags.None));
			_properties.Add(new FbxProperty<int>("OriginalUpAxis", null, 2, "int", "Integer", PropertyFlags.None));
			_properties.Add(new FbxProperty<int>("OriginalUpAxisSign", null, 1, "int", "Integer", PropertyFlags.None));
			_properties.Add(new FbxProperty<double>("UnitScaleFactor", null, 100000, "double", "Number", PropertyFlags.None));
			_properties.Add(new FbxProperty<double>("OriginalUnitScaleFactor", null, 100, "double", "Number", PropertyFlags.None));
			_properties.Add(new FbxProperty<Color>("AmbientColor", null, new Color(), "ColorRGB", "Color", PropertyFlags.None));
			_properties.Add(new FbxProperty<string>("DefaultCamera", null, "Producer", "KString", "", PropertyFlags.None));
			//_properties.Add(new FbxProperty<>("TimeMode", null, "enum", "", "", 6));
			//_properties.Add(new FbxProperty<>("TimeProtocol", null, "enum", "", "", 2));
			//_properties.Add(new FbxProperty<>("SnapOnFrameMode", null, "enum", "", "", 0));
			//_properties.Add(new FbxProperty<>("TimeSpanStart", null, "KTime", "Time", "", 0));
			//_properties.Add(new FbxProperty<>("TimeSpanStop", null, "KTime", "Time", "", 153953860));
			_properties.Add(new FbxProperty<double>("CustomFrameRate", null, -1, "double", "Number", PropertyFlags.None));
			//_properties.Add(new FbxProperty<>("TimeMarker", null, "Compound", "", ""));
			_properties.Add(new FbxProperty<int>("CurrentTimeMarker", null, -1, "int", "Integer", PropertyFlags.None));
		}

		[Obsolete]
		public FbxNode ToNode()
		{
			FbxNode node = new FbxNode("GlobalSettings");

			node.Nodes.Add(new FbxNode("Version", (int)Version));

			FbxNode properties = new FbxNode("Properties70");

			node.Nodes.Add(properties);

			properties.Nodes.Add(new FbxNode("P", "UpAxis", "int", "Integer", "", 1));
			properties.Nodes.Add(new FbxNode("P", "UpAxisSign", "int", "Integer", "", 1));
			properties.Nodes.Add(new FbxNode("P", "FrontAxis", "int", "Integer", "", 2));
			properties.Nodes.Add(new FbxNode("P", "FrontAxisSign", "int", "Integer", "", 1));
			properties.Nodes.Add(new FbxNode("P", "CoordAxis", "int", "Integer", "", 0));
			properties.Nodes.Add(new FbxNode("P", "CoordAxisSign", "int", "Integer", "", 1));
			properties.Nodes.Add(new FbxNode("P", "OriginalUpAxis", "int", "Integer", "", 2));
			properties.Nodes.Add(new FbxNode("P", "OriginalUpAxisSign", "int", "Integer", "", 1));
			properties.Nodes.Add(new FbxNode("P", "UnitScaleFactor", "double", "Number", "", 100));
			properties.Nodes.Add(new FbxNode("P", "OriginalUnitScaleFactor", "double", "Number"));
			properties.Nodes.Add(new FbxNode("P", "AmbientColor", "ColorRGB", "Color", "", 0, 0, 0));
			properties.Nodes.Add(new FbxNode("P", "DefaultCamera", "KString", "", "", "Producer"));
			properties.Nodes.Add(new FbxNode("P", "TimeMode", "enum", "", "", 6));
			properties.Nodes.Add(new FbxNode("P", "TimeProtocol", "enum", "", "", 2));
			properties.Nodes.Add(new FbxNode("P", "SnapOnFrameMode", "enum", "", "", 0));
			properties.Nodes.Add(new FbxNode("P", "TimeSpanStart", "KTime", "Time", "", 0));
			properties.Nodes.Add(new FbxNode("P", "TimeSpanStop", "KTime", "Time", "", 153953860));
			properties.Nodes.Add(new FbxNode("P", "CustomFrameRate", "double", "Number", "", -1));
			properties.Nodes.Add(new FbxNode("P", "TimeMarker", "Compound", "", ""));
			properties.Nodes.Add(new FbxNode("P", "CurrentTimeMarker", "int", "Integer", "", -1));

			return node;
		}

		private T getPropertyValue<T>([CallerMemberName] string name = null)
		{
			return (T)_properties[name].Value;
		}

		private void setPropertyValue(object value, [CallerMemberName] string name = null)
		{
			_properties[name].Value = value;
		}
	}
}
