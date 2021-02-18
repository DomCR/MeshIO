using MeshIO.FBX.Nodes.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.FBX.Nodes
{
	public class FbxObjectCollection : FbxNodeReferenceCollection<FbxObject>
	{
		public override string ClassName { get { return "Objects"; } }
		//****************************************************************
		public FbxObjectCollection() : base()
		{
		}
		public FbxObjectCollection(FbxNode node) : base(node)
		{

		}
		//****************************************************************
		public FbxObject GetById(ulong id)
		{
			throw new NotImplementedException();
		}
		public FbxObject GetByName(string name)
		{
			throw new NotImplementedException();
		}
		public List<T> GetFbxObjects<T>() where T : FbxObject
		{
			throw new NotImplementedException();
		}
		//****************************************************************
		protected override FbxObject buildChild(FbxNode node)
		{
			return FbxNodeBuilder.CreateFbxObject(node);
		}
	}
}