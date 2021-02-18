using MeshIO.FBX.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeshIO.FBX.Nodes
{

	public class FbxDefinitions : FbxNodeReferenceCollection<FbxObjectTypeNode>
	{
		public override string ClassName { get { return "Definitions"; } }
		[FbxChildNode("Count")]
		public int Count { get { return m_children.Count; } }

		public FbxDefinitions()
		{
		}

		public FbxDefinitions(FbxNode node)
		{
		}

		public void Add(string type)
		{
			FbxObjectTypeNode definition = m_children.FirstOrDefault(o => o.TypeName == type);
			if (definition == null)
				m_children.Add(new FbxObjectTypeNode { TypeName = type });
			else
				definition.Count++;
		}

		protected override FbxObjectTypeNode buildChild(FbxNode node)
		{
			return new FbxObjectTypeNode();
		}
	}
}
