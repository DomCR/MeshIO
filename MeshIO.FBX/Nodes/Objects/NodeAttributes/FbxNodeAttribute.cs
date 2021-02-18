using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.FBX.Nodes.Objects
{
	public abstract class FbxNodeAttribute : FbxObject
	{
		public override string ClassName { get { return "NodeAttribute"; } }
		public override FbxObjectType ObjectType
		{
			get { return m_objectType; }
			set
			{
				//Each type of a node attribute represents a class, cannot be assigned
				throw new InvalidOperationException("Cannot set the type in a node attribute.");
			}
		}
		protected abstract FbxObjectType m_objectType { get; }
		public FbxNodeAttribute() : base() { }
		public FbxNodeAttribute(FbxNode node) : base(node) { }
	}
}
