using CSMath;
using MeshIO.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MeshIO.FBX.Converters.Mappers
{
	public class FbxPropertyMapper
	{
		public event NotificationEventHandler OnNotification;

		public void MapNode()
		{

		}

		public static List<Property> MapProperties(FbxNode node, NotificationEventHandler notify = null)
		{
			List<Property> properties = new List<Property>();

			foreach (FbxNode n in node.Nodes)
			{
				var p = MapProperty(n);

				if (properties.Select(p => p.Name).Contains(p.Name))
				{
					notify?.Invoke(null, new NotificationEventArgs($"Duplicated property with name : {p.Name}"));
					continue;
				}

				properties.Add(p);
			}

			return properties;
		}

		public static Property MapProperty(FbxNode node)
		{
			//P : ["PropName", "PropType", "Label(?)", "Flags", __values__, …]
			Property property = null;

			string type1 = (string)node.Properties[1];
			string type2 = (string)node.Properties[2];
			string flags = (string)node.Properties[3];

			switch (type1)
			{
				case "Color":
				case "ColorRGB":
					byte r = (byte)(Convert.ToDouble(node.Properties[4]) * 255);
					byte g = (byte)(Convert.ToDouble(node.Properties[5]) * 255);
					byte b = (byte)(Convert.ToDouble(node.Properties[6]) * 255);
					property = new Property<Color>(node.Properties[0].ToString(), new Color(r, g, b));
					break;
				case "ColorAndAlpha":
					r = (byte)(Convert.ToDouble(node.Properties[4]) * 255);
					g = (byte)(Convert.ToDouble(node.Properties[5]) * 255);
					b = (byte)(Convert.ToDouble(node.Properties[6]) * 255);
					byte a = (byte)(Convert.ToDouble(node.Properties[7]) * 255);
					property = new Property<Color>(node.Properties[0].ToString(), new Color(r, g, b, a));
					break;
				case "Visibility":
				case "Bool":
				case "bool":
					property = new Property<bool>(node.Properties[0].ToString(), Convert.ToInt32(node.Properties[4]) != 0);
					break;
				case "Vector":
				case "Vector3":
				case "Vector3D":
				case "Lcl Translation":
				case "Lcl Rotation":
				case "Lcl Scaling":
					double x = Convert.ToDouble(node.Properties[4]);
					double y = Convert.ToDouble(node.Properties[5]);
					double z = Convert.ToDouble(node.Properties[6]);
					property = new Property<XYZ>(node.Properties[0].ToString(), new XYZ(x, y, z));
					break;
				case "int":
				case "Integer":
				case "Enum":
				case "enum":
					property = new Property<int>(node.Properties[0].ToString(), Convert.ToInt32(node.Properties[4]));
					break;
				case "KString":
					property = new Property<string>(node.Properties[0].ToString(), (string)node.Properties[4]);
					break;
				case "Float":
					property = new Property<float>(node.Properties[0].ToString(), Convert.ToSingle(node.Properties[4]));
					break;
				case "FieldOfView":
				case "FieldOfViewX":
				case "FieldOfViewY":
				case "double":
				case "Number":
					property = new Property<double>(node.Properties[0].ToString(), Convert.ToDouble(node.Properties[4]));
					break;
				case "KTime":
					property = new Property<TimeSpan>(node.Properties[0].ToString(), new TimeSpan(Convert.ToInt64(node.Properties[4])));
					break;
				case "Reference":
				case "Compound":
					property = new Property(node.Properties[0].ToString(), null);
					break;
				default:
					System.Diagnostics.Debug.Fail($"{node.Properties[1]}");
					break;
			}

			return property;
		}

		protected void notify(string message, [CallerMemberName] string caller = null)
		{
			this.OnNotification?.Invoke(this, new NotificationEventArgs($"{caller} | {message}"));
		}
	}
}
