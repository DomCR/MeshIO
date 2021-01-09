using System;

namespace MeshIO.FBX.Attributes
{
	[System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
	internal class FbxChildNodeAttribute : Attribute
	{
		public string Name { get; set; }
		public FbxChildNodeAttribute() { }
		public FbxChildNodeAttribute(string name)
		{
			Name = name;
		}
	}
}