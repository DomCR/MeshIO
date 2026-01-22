using MeshIO.Formats.Fbx.Writers;

namespace MeshIO.Formats.Fbx.Templates;

internal interface IFbxObjectTemplate
{
	string FbxObjectName { get; }

	string FbxTypeName { get; }

	string Id { get; set; }

	string Name { get; }

	void ApplyTemplate(FbxPropertyTemplate template);

	Element3D GetElement();

	string GetIdByVersion(FbxVersion version);

	void ProcessChildren(FbxFileWriterBase fbxFileWriterBase);

	FbxNode ToFbxNode(FbxFileWriterBase writer);
}