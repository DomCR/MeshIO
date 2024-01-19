namespace MeshIO.FBX.Writers.Objects
{
	internal interface IFbxObjectWriter
	{
		ulong Id { get; }

		string Name { get; }

		string FbxObjectName { get; }

		string FbxTypeName { get; }

		void ApplyTemplate(FbxPropertyTemplate template);

		void Write(FbxFileWriterBase fbxFileWriterBase, IFbxStreamWriter writer);

		void ProcessChildren(FbxFileWriterBase fbxFileWriterBase);
	}
}
