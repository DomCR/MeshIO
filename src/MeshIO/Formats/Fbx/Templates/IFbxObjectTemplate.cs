using MeshIO.Formats.Fbx.Readers;
using MeshIO.Formats.Fbx.Writers;

namespace MeshIO.Formats.Fbx.Templates
{
	internal interface IFbxObjectTemplate
	{
		string Id { get; }

		string Name { get; }

		string FbxObjectName { get; }

		string FbxTypeName { get; }

		Element3D GetElement();

		void Build(FbxFileBuilderBase builder);
		
		FbxNode ToFbxNode(FbxFileWriterBase writer);

		void ProcessChildren(FbxFileWriterBase fbxFileWriterBase);

		void ApplyTemplate(FbxPropertyTemplate template);
	}
}
