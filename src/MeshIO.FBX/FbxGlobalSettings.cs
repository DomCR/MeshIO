using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MeshIO.FBX
{
	/// <summary>
	/// Global settings to be setup in the fbx file
	/// </summary>
	public class FbxGlobalSettings
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

		public IEnumerable<FbxProperty> Properties { get { return this._properties.Values; } }

		private Dictionary<string, FbxProperty> _properties { get; } = new();

		public FbxGlobalSettings(FbxVersion version)
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

			this.add(new("UpAxis", "int", "Integer", PropertyFlags.None, 1));
			this.add(new("UpAxisSign", "int", "Integer", PropertyFlags.None, 1));
			this.add(new("FrontAxis", "int", "Integer", PropertyFlags.None, 2));
			this.add(new("FrontAxisSign", "int", "Integer", PropertyFlags.None, 1));
			this.add(new("CoordAxis", "int", "Integer", PropertyFlags.None, 0));
			this.add(new("CoordAxisSign", "int", "Integer", PropertyFlags.None, 1));
			this.add(new("OriginalUpAxis", "int", "Integer", PropertyFlags.None, 2));
			this.add(new("OriginalUpAxisSign", "int", "Integer", PropertyFlags.None, 1));
			this.add(new("UnitScaleFactor", "double", "Number", PropertyFlags.None, 1.0d));
			this.add(new("OriginalUnitScaleFactor", "double", "Number", PropertyFlags.None, 1.0d));
			this.add(new("AmbientColor", "ColorRGB", "Color", PropertyFlags.None, new Color()));
			this.add(new("DefaultCamera", "KString", string.Empty, PropertyFlags.None, "Producer Perspective"));
			this.add(new("TimeMode", "enum", string.Empty, PropertyFlags.None, 6));
			this.add(new("TimeProtocol", "enum", string.Empty, PropertyFlags.None, 2));
			this.add(new("SnapOnFrameMode", "enum", string.Empty, PropertyFlags.None, 0));
			this.add(new("TimeSpanStart", "KTime", "Time", PropertyFlags.None, 0));
			this.add(new("TimeSpanStop", "KTime", "Time", PropertyFlags.None, 153953860000));
			this.add(new("CustomFrameRate", "double", "Number", PropertyFlags.None, -1.0d));
			this.add(new("TimeMarker", "Compound", string.Empty, PropertyFlags.None, string.Empty));
			this.add(new("CurrentTimeMarker", "int", "Integer", PropertyFlags.None, -1));
		}

		private void add(FbxProperty property)
		{
			this._properties.Add(property.Name, property);
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
