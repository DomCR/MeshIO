using System;

namespace MeshIO.FBX
{
	//P : ["PropName", "PropType", "Label(?)", "Flags", __values__, …]
	[Obsolete]
	public class FbxPropertyOld<T> : FbxPropertyOld
	{
		public new T Value { get; set; }

		public FbxPropertyOld(string name, Element3D owner) : base(name, owner) { }

		public FbxPropertyOld(string name, Element3D owner, T value) : base(name, owner, value) { }

		public FbxPropertyOld(string name, Element3D owner, T value, string typeName, string typeLabel, PropertyFlags flags) : base(name, owner, value, typeName, typeLabel, flags) { }
	}
}
