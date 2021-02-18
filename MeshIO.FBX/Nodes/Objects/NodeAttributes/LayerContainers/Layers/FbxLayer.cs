using System.Collections.Generic;

namespace MeshIO.FBX.Nodes.Objects.NodeAttributes.LayerContainers
{
	public class FbxLayer
	{
		public int Index { get; }
		public Dictionary<string, FbxLayerElement> Elements { get; set; } = new Dictionary<string, FbxLayerElement>();

		public FbxLayer(int index)
		{
			Index = index;
		}
		public void AddElement(FbxLayerElement element)
		{
			Elements.Add(element.TypeName, element);
		}
		public List<FbxNode> ToFbxNodes()
		{
			List<FbxNode> layerElements = new List<FbxNode>();
			FbxNode layer = new FbxNode("Layer", Index);
			layerElements.Add(layer);

			foreach (var item in Elements.Values)
			{
				//Add the element descriptor to the layer node
				layer.Nodes.Add(item.ToLayerElementNode(Index));

				//Add the layer element to the list of nodes
				FbxNode layerElement = item.ToFbxNode(Index);
				layerElements.Add(layerElement);
			}

			return layerElements;
		}
	}
}
