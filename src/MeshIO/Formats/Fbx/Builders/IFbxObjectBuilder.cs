using MeshIO.Formats.Fbx.Readers;
using MeshIO.Formats.Fbx.Writers;

namespace MeshIO.Formats.Fbx.Builders;

internal interface IFbxObjectBuilder
{
	string Id { get; set; }

	string Name { get; }

	string FbxObjectName { get; }

	string FbxTypeName { get; }

	Element3D GetElement();

	void Build(FbxFileBuilderBase builder);
	
	FbxNode ToFbxNode(FbxFileWriterBase writer);

	void ProcessChildren(FbxFileWriterBase fbxFileWriterBase);

	void ApplyTemplate(FbxPropertyBuilder template);
}
