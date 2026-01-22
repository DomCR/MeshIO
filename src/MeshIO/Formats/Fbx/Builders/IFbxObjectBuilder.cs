using MeshIO.Formats.Fbx.Readers;

namespace MeshIO.Formats.Fbx.Builders;

internal interface IFbxObjectBuilder
{
	string FbxObjectName { get; }

	string FbxTypeName { get; }

	string Id { get; set; }

	void Build(FbxFileBuilderBase builder);

	Element3D GetElement();
}