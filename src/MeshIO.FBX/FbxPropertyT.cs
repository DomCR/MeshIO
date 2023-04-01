namespace MeshIO.FBX
{
	//P : ["PropName", "PropType", "Label(?)", "Flags", __values__, …]
	public class FbxProperty<T> : FbxProperty
	{
		public new T Value { get; set; }

		public FbxProperty(string name, Element3D owner) : base(name, owner) { }

		public FbxProperty(string name, Element3D owner, T value) : base(name, owner, value) { }

		public FbxProperty(string name, Element3D owner, T value, string typeName, string typeLabel, PropertyFlags flags) : base(name, owner, value, typeName, typeLabel, flags) { }
	}
}
