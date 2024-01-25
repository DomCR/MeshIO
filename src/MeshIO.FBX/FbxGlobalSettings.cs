using System.Runtime.CompilerServices;

namespace MeshIO.FBX
{
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

		private PropertyCollection _properties = new PropertyCollection(null);

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

			_properties.Add(new FbxPropertyOld<int>("UpAxis", null, 1, "int", "Integer", PropertyFlags.None));
			_properties.Add(new FbxPropertyOld<int>("UpAxisSign", null, 1, "int", "Integer", PropertyFlags.None));
			_properties.Add(new FbxPropertyOld<int>("FrontAxis", null, 2, "int", "Integer", PropertyFlags.None));
			_properties.Add(new FbxPropertyOld<int>("FrontAxisSign", null, 1, "int", "Integer", PropertyFlags.None));
			_properties.Add(new FbxPropertyOld<int>("CoordAxis", null, 0, "int", "Integer", PropertyFlags.None));
			_properties.Add(new FbxPropertyOld<int>("CoordAxisSign", null, 1, "int", "Integer", PropertyFlags.None));
			_properties.Add(new FbxPropertyOld<int>("OriginalUpAxis", null, 2, "int", "Integer", PropertyFlags.None));
			_properties.Add(new FbxPropertyOld<int>("OriginalUpAxisSign", null, 1, "int", "Integer", PropertyFlags.None));
			_properties.Add(new FbxPropertyOld<double>("UnitScaleFactor", null, 100000, "double", "Number", PropertyFlags.None));
			_properties.Add(new FbxPropertyOld<double>("OriginalUnitScaleFactor", null, 100, "double", "Number", PropertyFlags.None));
			_properties.Add(new FbxPropertyOld<Color>("AmbientColor", null, new Color(), "ColorRGB", "Color", PropertyFlags.None));
			_properties.Add(new FbxPropertyOld<string>("DefaultCamera", null, "Producer", "KString", "", PropertyFlags.None));
			//_properties.Add(new FbxProperty<>("TimeMode", null, "enum", "", "", 6));
			//_properties.Add(new FbxProperty<>("TimeProtocol", null, "enum", "", "", 2));
			//_properties.Add(new FbxProperty<>("SnapOnFrameMode", null, "enum", "", "", 0));
			//_properties.Add(new FbxProperty<>("TimeSpanStart", null, "KTime", "Time", "", 0));
			//_properties.Add(new FbxProperty<>("TimeSpanStop", null, "KTime", "Time", "", 153953860));
			_properties.Add(new FbxPropertyOld<double>("CustomFrameRate", null, -1, "double", "Number", PropertyFlags.None));
			//_properties.Add(new FbxProperty<>("TimeMarker", null, "Compound", "", ""));
			_properties.Add(new FbxPropertyOld<int>("CurrentTimeMarker", null, -1, "int", "Integer", PropertyFlags.None));
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
