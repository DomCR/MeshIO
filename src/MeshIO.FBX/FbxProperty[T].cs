using MeshIO.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.FBX
{
	//P : ["PropName", "PropType", "Label(?)", "Flags", __values__, …]
	public class FbxProperty<T> : FbxProperty
	{
		public new T Value { get; set; }

		public FbxProperty(string name, Element owner) : base(name, owner) { }

		public FbxProperty(string name, Element owner, T value) : base(name, owner, value) { }

		public FbxProperty(string name, Element owner, T value, string typeName, string typeLabel, PropertyFlags flags) : base(name, owner, value, typeName, typeLabel, flags) { }
	}
}
