using System;

namespace MeshIO.FBX.Attributes
{
	[System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
	internal class FbxValueReferenceAttribute : Attribute
	{
		public string Name { get; set; }
		public FbxValueReferenceAttribute() { }
		public FbxValueReferenceAttribute(string name)
		{
			Name = name;
		}
	}
}