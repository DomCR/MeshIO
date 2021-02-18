using System;

namespace MeshIO.FBX.Nodes.Objects.NodeAttributes.LayerContainers
{
	public abstract class FbxLayerElement
	{
		public abstract string TypeName { get; }
		public string Name { get; set; } = string.Empty;
		public MappingInformationType MappingInformationType { get; set; }
		public ReferenceInformationType ReferenceInformationType { get; set; }
		/*
		 	LayerElement:  {
				Type: "LayerElementNormal"
				TypedIndex: 0
			}
		 */
		public FbxNode ToLayerElementNode(int index)
		{
			FbxNode node = new FbxNode("LayerElement");

			node.Nodes.Add(new FbxNode("Type", TypeName));
			node.Nodes.Add(new FbxNode("TypedIndex", index));

			return node;
		}
		public virtual FbxNode ToFbxNode(int index)
		{
			FbxNode node = new FbxNode(TypeName, index);

			node.Nodes.Add(new FbxNode("Name", Name));
			node.Nodes.Add(new FbxNode("MappingInformationType", MappingInformationType.ToString()));
			node.Nodes.Add(new FbxNode("ReferenceInformationType", ReferenceInformationType.ToString()));

			return node;
		}
		public virtual FbxNode ToFbxNode(FbxVersion version)
		{
			throw new NotImplementedException();
		}
	}

	/*
	 	LayerElementMaterial: 0 {
			Version: 101
			Name: ""
			MappingInformationType: "AllSame"
			ReferenceInformationType: "IndexToDirect"
			Materials: *1 {
				a: 0
			} 
		}
	 */
	public class FbxLayerElementMaterial : FbxLayerElement
	{
		//TODO: FbxLayerElementMaterial implement the class properly
		public override string TypeName { get { return "LayerElementMaterial"; } }
		public int[] Materials { get; set; } = new int[] { 0 };
		public FbxLayerElementMaterial()
		{
			MappingInformationType = MappingInformationType.AllSame;
			ReferenceInformationType = ReferenceInformationType.IndexToDirect;
		}
		public override FbxNode ToFbxNode(int index)
		{
			FbxNode node = base.ToFbxNode(index);

			node.Nodes.Add(new FbxNode("Materials", Materials));

			return node;
		}
	}
}
