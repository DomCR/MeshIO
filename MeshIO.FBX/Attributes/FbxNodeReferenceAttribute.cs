using System;

namespace MeshIO.FBX.Attributes
{
	[System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
	internal class FbxNodeReferenceAttribute : Attribute
	{
		public string Name { get; set; }
		public FbxNodeReferenceAttribute() { }
		public FbxNodeReferenceAttribute(string name)
		{
			Name = name;
		}
	}
}