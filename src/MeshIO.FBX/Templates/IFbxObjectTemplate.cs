using MeshIO.FBX.Readers;
using MeshIO.FBX.Writers;

namespace MeshIO.FBX.Templates
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
