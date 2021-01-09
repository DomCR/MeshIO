using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.FBX.Nodes.Properties
{
	//P : ["PropName", "PropType", "Label(?)", "Flags", __values__, …]
	public class FbxProperty : FbxNodeReference, IEquatable<FbxProperty>
	{
		public override string ClassName { get { return "P"; } }
		public string Name { get; set; }
		public string TypeName { get; set; }
		public string Label { get; set; }
		public FbxPropertyFlag Flags { get; set; }
		public object[] Values { get; set; }

		public bool Equals(FbxProperty other)
		{
			return other.Name == this.Name;
		}
		public override bool Equals(object obj)
		{
			if (obj is FbxProperty property)
				return this.Equals(property);
			else
				return false;
		}
		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}
	}
}
