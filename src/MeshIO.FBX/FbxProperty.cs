using CSMath;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO.FBX
{
	public class FbxProperty : Property
	{
		public static class Geometry
		{
			public const string Color = "Color";
			public const string BBoxMin = "BBoxMin";
			public const string BBoxMax = "BBoxMax";
			public const string PrimaryVisibility = "Primary Visibility";
			public const string CastsShadows = "Casts Shadows";

			//public static FbxProperty<bool> CreatePrimaryVisibility() => new FbxProperty<bool>(PrimaryVisibility, true);
		}

		/// <summary>
		/// Fbx equivalent type name
		/// </summary>
		public string FbxType { get; }

		/// <summary>
		/// Fbx label
		/// </summary>
		public string Label { get; }

		public FbxProperty(string name, PropertyFlags flags, object value) : this(name, string.Empty, string.Empty, flags, value)
		{
			GetFbxValue(value, out string fbxtype, out string label);
			this.FbxType = fbxtype;
			this.Label = label;
		}

		public FbxProperty(string name, string fbxtype, string label, PropertyFlags flags, object value) : base(name)
		{
			this.FbxType = fbxtype;
			this.Label = label;
			this.Flags = flags;

			if (value is byte b)
			{
				this.Value = (int)b;
			}
			else
			{
				this.Value = value;
			}
		}

		public FbxProperty(FbxProperty property, object value) : this(property.Name, property.FbxType, property.Label, property.Flags, value)
		{
		}

		/// <summary>
		/// Create a <see cref="FbxProperty"/> based on a <see cref="Property"/>
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		public static FbxProperty CreateFrom(Property property)
		{
			return new FbxProperty(property.Name, property.Flags, property.Value);
		}

		public Property ToProperty()
		{
			Property property = null;

			object value = this.Value;
			List<object> arr = null;
			if (value is IEnumerable<object> en)
			{
				arr = new List<object>(en);
			}

			switch (FbxType)
			{
				case "Color":
				case "ColorRGB":
					byte r = (byte)(Convert.ToDouble(arr[0]) * 255);
					byte g = (byte)(Convert.ToDouble(arr[1]) * 255);
					byte b = (byte)(Convert.ToDouble(arr[2]) * 255);
					property = new Property<Color>(this.Name, Flags, new Color(r, g, b));
					break;
				case "ColorAndAlpha":
					r = (byte)(Convert.ToDouble(arr[0]) * 255);
					g = (byte)(Convert.ToDouble(arr[1]) * 255);
					b = (byte)(Convert.ToDouble(arr[2]) * 255);
					byte a = (byte)(Convert.ToDouble(arr[3]) * 255);
					property = new Property<Color>(this.Name, Flags, new Color(r, g, b, a));
					break;
				case "Visibility":
				case "Bool":
				case "bool":
					property = new Property<bool>(this.Name, Flags, Convert.ToInt32(value) != 0);
					break;
				case "Vector":
				case "Vector3":
				case "Vector3D":
				case "Lcl Translation":
				case "Lcl Rotation":
				case "Lcl Scaling":
					double x = Convert.ToDouble(arr[0]);
					double y = Convert.ToDouble(arr[1]);
					double z = Convert.ToDouble(arr[2]);
					property = new Property<XYZ>(this.Name, Flags, new XYZ(x, y, z));
					break;
				case "int":
				case "Integer":
				case "Enum":
				case "enum":
					property = new Property<int>(this.Name, Flags, Convert.ToInt32(value));
					break;
				case "KString":
					property = new Property<string>(this.Name, Flags, (string)value);
					break;
				case "Float":
					property = new Property<float>(this.Name, Flags, Convert.ToSingle(value));
					break;
				case "FieldOfView":
				case "FieldOfViewX":
				case "FieldOfViewY":
				case "double":
				case "Number":
					property = new Property<double>(this.Name, Flags, Convert.ToDouble(value));
					break;
				case "KTime":
					property = new Property<TimeSpan>(this.Name, Flags, new TimeSpan(Convert.ToInt64(value)));
					break;
				case "Reference":
				case "Compound":
				case "object":
				default:
					property = new Property(this.Name, Flags, value);
					break;
			}

			return property;
		}

		public FbxNode ToNode()
		{
			//P : ["PropName", "PropType", "Label(?)", "Flags", __values__, …]
			FbxNode p = new FbxNode("P", this.Name, this.FbxType, this.Label, MapPropertyFlags(this.Flags));

			switch (this.Value)
			{
				case string:
				case double:
				case int:
				case long:
				case float:
					p.Properties.Add(this.Value);
					break;
				case Color value:
					if (value.A.HasValue)
					{
						p.Properties.Add(value.R / (double)255);
						p.Properties.Add(value.G / (double)255);
						p.Properties.Add(value.B / (double)255);
						p.Properties.Add(value.A / (double)255);
					}
					else
					{
						p.Properties.Add(value.R / (double)255);
						p.Properties.Add(value.G / (double)255);
						p.Properties.Add(value.B / (double)255);
					}
					break;
				case bool value:
					p.Properties.Add(value ? 1 : 0);
					break;
				case XYZ value:
					p.Properties.AddRange(value.ToEnumerable().Cast<object>());
					break;
				case null:
					break;
				default:
					System.Diagnostics.Debug.Fail($"{Value.GetType().FullName}");
					break;
			}

			return p;
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
				case byte value:
					fbxtype = "int";
					label = "Integer";
					return (int)value;
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
