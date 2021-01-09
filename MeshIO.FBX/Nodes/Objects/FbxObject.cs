using MeshIO.FBX.Attributes;
using MeshIO.FBX.Nodes.Properties;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MeshIO.FBX.Nodes.Objects
{
	/* 
	 * [Class]: [Class::Name], [ObjectType] { }	//v 6000  
	 * [Class]: [ID], [Class::Name], [ObjectType] { }	//v 7000 or higher 
	 */
	public abstract class FbxObject : FbxNodeReference
	{
		public ulong Id { get; set; }
		public string Name { get; set; }
		public string FullName { get { return $"{ClassName}::{Name}"; } }
		public virtual FbxObjectType ObjectType { get; set; }

		[FbxChildNode("Properties70")]	//TODO: implement a "get name by version"
		public FbxPropertyCollection Properties { get; set; } = new FbxPropertyCollection();
		//****************************************************************
		public FbxObject()
		{
			Id = NodeUtils.CreateId();
		}
		public FbxObject(FbxNode node) : base(node)
		{

		}
		//****************************************************************
		public override FbxNode ToFbxNode()
		{
			FbxNode node = base.ToFbxNode();

			node.Properties.Add(Id);
			node.Properties.Add(FullName);
			node.Properties.Add(ObjectType.ToString());

			return node;
		}
		//*********************************************************************
	}
}
