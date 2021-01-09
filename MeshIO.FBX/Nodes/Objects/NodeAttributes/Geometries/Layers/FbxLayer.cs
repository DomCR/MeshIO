using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.FBX.Nodes.Objects.NodeAttributes.Geometries.Layers
{
	public abstract class FbxLayer : FbxNodeReference
	{
		public uint TypeIndex { get; set; }
		public string Name { get; set; }
		public MappingInformationType MappingInformationType { get; set; }
		public ReferenceInformationType ReferenceInformationType { get; set; }
	}
}
