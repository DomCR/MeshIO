using MeshIO.Formats.Fbx.Writers;

namespace MeshIO.Formats.Fbx.Templates;

internal interface IFbxObjectTemplate
{
	string Id { get; set; }

	string Name { get; }

	string FbxObjectName { get; }

	string FbxTypeName { get; }

	Element3D GetElement();

	FbxNode ToFbxNode(FbxFileWriterBase writer);

	void ProcessChildren(FbxFileWriterBase fbxFileWriterBase);

	void ApplyTemplate(FbxPropertyTemplate template);
}
