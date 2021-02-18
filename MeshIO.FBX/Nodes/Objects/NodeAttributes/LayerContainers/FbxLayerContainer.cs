using MeshIO.FBX.Nodes.Objects.NodeAttributes.LayerContainers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.FBX.Nodes.Objects
{
	/*	LayerElementNormal: 0 {
			Version: 102
			Name: ""
			MappingInformationType: "ByPolygonVertex"
			ReferenceInformationType: "Direct"
			Normals: *108 {
				a: 0,0,-1,0,0,-1,0,0,-1,0,0,-1,0,0,-1,0,0,-1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,-1,0,0,-1,0,0,-1,0,0,-1,0,0,-1,0,0,-1,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,-1,0,0,-1,0,0,-1,0,0,-1,0,0,-1,0,0,-1,0,0
			} 
			NormalsW: *36 {
				a: 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
			} 
		}
		LayerElementBinormal: 0 {
			Version: 102
			Name: "UVChannel_1"
			MappingInformationType: "ByPolygonVertex"
			ReferenceInformationType: "Direct"
			Binormals: *108 {
				a: 0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,-0,0,1,-0,0,1,-0,0,1,-0,0,1,-0,0,1,-0,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,-0,0,1,-0,0,1,-0,0,1,-0,0,1,-0,0,1,-0,0,1,0,-0,1,0,-0,1,0,-0,1,0,-0,1,0,-0,1,0,-0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1
			} 
			BinormalsW: *36 {
				a: 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1
			} 

		}
		LayerElementTangent: 0 {
			Version: 102
			Name: "UVChannel_1"
			MappingInformationType: "ByPolygonVertex"
			ReferenceInformationType: "Direct"
			Tangents: *108 {
				a: -1,-0,0,-1,-0,0,-1,-0,0,-1,-0,0,-1,-0,0,-1,-0,0,1,-0,-0,1,-0,-0,1,-0,-0,1,-0,-0,1,-0,-0,1,-0,-0,1,0,-0,1,0,-0,1,0,-0,1,0,-0,1,0,-0,1,0,-0,-0,1,-0,-0,1,-0,-0,1,-0,-0,1,-0,-0,1,-0,-0,1,-0,-1,0,0,-1,0,0,-1,0,0,-1,0,0,-1,0,0,-1,0,0,0,-1,-0,0,-1,-0,0,-1,-0,0,-1,-0,0,-1,-0,0,-1,-0
			} 
			TangentsW: *36 {
				a: 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1
			} 
		}
		LayerElementUV: 0 {
			Version: 101
			Name: "UVChannel_1"
			MappingInformationType: "ByPolygonVertex"
			ReferenceInformationType: "IndexToDirect"
			UV: *48 {
				a: 1,0,0,0,1,1,0,1,0,0,1,0,0,1,1,1,0,0,1,0,0,1,1,1,0,0,1,0,0,1,1,1,0,0,1,0,0,1,1,1,0,0,1,0,0,1,1,1
			} 
			UVIndex: *36 {
				a: 1,0,3,0,2,3,7,6,4,7,4,5,11,10,8,11,8,9,15,14,12,15,12,13,19,18,16,19,16,17,23,22,20,23,20,21
			} 
		}
		LayerElementSmoothing: 0 {
			Version: 102
			Name: ""
			MappingInformationType: "ByPolygon"
			ReferenceInformationType: "Direct"
			Smoothing: *12 {
				a: 2,2,4,4,8,8,16,16,32,32,64,64
			} 
		}
		LayerElementMaterial: 0 {
			Version: 101
			Name: ""
			MappingInformationType: "AllSame"
			ReferenceInformationType: "IndexToDirect"
			Materials: *1 {
				a: 0
			} 
		}
		Layer: 0 {
			Version: 100
			LayerElement:  {
				Type: "LayerElementNormal"
				TypedIndex: 0
			}
			LayerElement:  {
				Type: "LayerElementBinormal"
				TypedIndex: 0
			}
			LayerElement:  {
				Type: "LayerElementTangent"
				TypedIndex: 0
			}
			LayerElement:  {
				Type: "LayerElementMaterial"
				TypedIndex: 0
			}
			LayerElement:  {
				Type: "LayerElementSmoothing"
				TypedIndex: 0
			}
			LayerElement:  {
				Type: "LayerElementUV"
				TypedIndex: 0
			}
		}	 
	 */
	public abstract class FbxLayerContainer : FbxNodeAttribute
	{
		public Dictionary<int, FbxLayer> Layers { get; set; } = new Dictionary<int, FbxLayer>();

		public FbxLayerContainer() : base()
		{
			//Add the default layer
			AddLayer(new FbxLayer(0));
		}
		public FbxLayerContainer(FbxNode node) : base(node) { }
		public void AddLayer(FbxLayer layer)
		{
			Layers.Add(layer.Index, layer);
		}
		public override FbxNode ToFbxNode()
		{
			FbxNode node = base.ToFbxNode();

			foreach (FbxLayer item in Layers.Values)
			{
				node.Nodes.AddRange(item.ToFbxNodes());
			}

			return node;
		}
	}
}
