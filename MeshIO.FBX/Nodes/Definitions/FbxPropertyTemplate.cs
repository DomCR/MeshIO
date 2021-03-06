﻿using MeshIO.FBX.Attributes;
using MeshIO.FBX.Nodes.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.FBX.Nodes
{
	public class FbxPropertyTemplate : FbxEmitter
	{
		public override string ClassName { get { return "PropertyTemplate"; } }
		public string ElementName { get; set; }
		[FbxChildNode]
		public FbxPropertyCollection Properties { get; set; } = new FbxPropertyCollection();
	}
}
