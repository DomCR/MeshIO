namespace MeshIO.FBX.Templates
{
    internal struct FbxConnectionTemplate
    {
        public string ParentId { get; }

        public string ChildId { get; }

        public FbxConnectionTemplate(string parentId, string childId)
        {
            ParentId = parentId;
            ChildId = childId;
        }
    }
}
