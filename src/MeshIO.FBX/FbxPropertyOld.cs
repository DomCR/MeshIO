using CSMath;
using System;

namespace MeshIO.FBX
{
	//P : ["PropName", "PropType", "Label(?)", "Flags", __values__, …]
	[Obsolete]
	public class FbxPropertyOld : Property
	{
		//Model porperties
		public const string LclTranslation = "Lcl Translation";
		public const string LclScaling = "Lcl Scaling";
		public const string LclRotation = "Lcl Rotation";

		public const string CastShadows = "Casts Shadows";
		public const string ReceiveShadows = "Receive Shadows";
		public const string PrimaryVisibility = "Primary Visibility";
		public const string Show = "Show";  //IsVisible
		public const string Freeze = "Freeze";  //

		//Material Properties
		public const string AmbientColor = "AmbientColor";

		public static readonly System.Text.RegularExpressions.Regex PropertiesRegex = new System.Text.RegularExpressions.Regex(@"(Properties).*?[\d]+");

		public string FbxTypeName { get; set; }

		public string TypeLabel { get; set; }

		public FbxPropertyOld(string name, Element3D owner) : base(name, owner) { }

		public FbxPropertyOld(string name, Element3D owner, object value) : base(name, owner, value) { }

		public FbxPropertyOld(string name, Element3D owner, object value, string typeName, string typeLabel, PropertyFlags flags) : base(name, owner, value)
		{
			this.FbxTypeName = typeName;
			this.TypeLabel = typeLabel;
			this.Flags = flags;
		}
	}

	public class FbxProperty : Property
	{
		/// <summary>
		/// Fbx equivalent type name
		/// </summary>
		public string FbxType { get; }

		/// <summary>
		/// Fbx label
		/// </summary>
		public string Label { get; }

		public FbxProperty(string name, object value) : base(name)
		{
			this.Value = value;
			GetFbxValue(value, out string fbxtype, out string label);
			this.FbxType = fbxtype;
			this.Label = label;
		}

		public FbxProperty(string name, string fbxtype, string label, PropertyFlags flags, object value) : base(name)
		{
			this.FbxType = fbxtype;
			this.Label = label;
			this.Flags = flags;
			this.Value = value;
		}

		public FbxProperty(FbxProperty property, object value) : this(property.Name, property.FbxType, property.Label, property.Flags, value)
		{
		}

		/// <summary>
		/// Create a <see cref="FbxProperty"/> based on a <see cref="Property"/>
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public static FbxProperty CreateFrom(Property property)
		{
			throw new NotImplementedException();
		}

		public object GetFbxValue()
		{
			return GetFbxValue(this.Value, out _, out _);
		}

		public static object GetFbxValue(object pvalue, out string fbxtype, out string label)
		{
			switch (pvalue)
			{
				case string value:
					fbxtype = "KString";
					label = string.Empty;
					return value;
				case Color value:
					if (!value.A.HasValue)
					{
						fbxtype = "ColorRGB";
						label = "Color";

						var arr = new double[]
						{
							value.R / (double)255,
							value.G / (double)255,
							value.B / (double)255,
						};

						return arr;
					}
					else
					{
						fbxtype = "ColorAndAlpha";
						label = string.Empty;

						var arr = new double[]
						{
							value.R / (double)255,
							value.G / (double)255,
							value.B / (double)255,
							value.A.Value / (double)255,
						};

						return arr;
					}
				case double value:
					fbxtype = "double";
					label = "Number";
					return value;
				case int value:
					fbxtype = "int";
					label = "Integer";
					return value;
				case long value:
					fbxtype = string.Empty;
					label = string.Empty;
					return value;
				case float value:
					fbxtype = "Float";
					label = string.Empty;
					return value;
				case bool value:
					fbxtype = "bool";
					label = string.Empty;
					return value ? 1 : 0;
				case XYZ value:
					fbxtype = "Vector3D";
					label = "Vector";
					double[] xyz =
					{
						value.X,
						value.Y,
						value.Z
					};
					return xyz;
				default:
					throw new NotImplementedException($"Unknown FbxProperty type : {pvalue.GetType().FullName}");
			}
		}

		public static string MapPropertyFlags(PropertyFlags flags)
		{
			System.Text.StringBuilder str = new System.Text.StringBuilder();

			if ((flags & PropertyFlags.Animatable) != 0)
			{
				str.Append('A');
			}
			if ((flags & PropertyFlags.Animated) != 0)
			{
				str.Append('+');
			}
			if ((flags & PropertyFlags.UserDefined) != 0)
			{
				str.Append('U');
			}
			if ((flags & PropertyFlags.Hidden) != 0)
			{
				str.Append('H');
			}
			return str.ToString();
		}

		public static PropertyFlags ParseFlags(string value)
		{
			PropertyFlags flags = PropertyFlags.None;

			if (string.IsNullOrEmpty(value))
			{
				return PropertyFlags.None;
			}

			int i = 0;
			for (; i < value.Length; i++)
			{
				char c = value[i];
				switch (c)
				{
					case 'A':
						flags |= PropertyFlags.Animatable;
						break;
					case '+':
						flags |= PropertyFlags.Animated;
						break;
					case 'H':
						flags |= PropertyFlags.Hidden;
						break;
					case 'U':
						flags |= PropertyFlags.UserDefined;
						break;
					case 'L':
					case 'N':
						break;
				}
			}
			return flags;
		}
	}
}
