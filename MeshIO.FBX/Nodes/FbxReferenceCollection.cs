namespace MeshIO.FBX.Nodes
{
	public class FbxReference : FbxEmitter
	{
		public override string ClassName => "Reference";
	}

	public class FbxReferenceCollection : FbxNodeReferenceCollection<FbxReference>
	{
		public override string ClassName => "References";
		public FbxReferenceCollection() { }
		public FbxReferenceCollection(FbxNode node) { }
		protected override FbxReference buildChild(FbxNode node)
		{
			return new FbxReference();
		}
	}
}
