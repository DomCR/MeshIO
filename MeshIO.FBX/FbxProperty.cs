using MeshIO.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.FBX
{
	//P : ["PropName", "PropType", "Label(?)", "Flags", __values__, …]

	public class FbxProperty : Property
	{
		public PropertyFlags Flags { get; set; }

		public string FbxTypeName { get; set; }

		public string TypeLabel { get; set; }

		public FbxProperty(string name, Element owner) : base(name, owner) { }

		public FbxProperty(string name, Element owner, object value) : base(name, owner, value) { }
	}

	public class FbxProperty<T> : FbxProperty
	{
		public new T Value { get; set; }

		public FbxProperty(string name, Element owner) : base(name, owner) { }

		public FbxProperty(string name, Element owner, T value) : base(name, owner, value) { }
	}
}
