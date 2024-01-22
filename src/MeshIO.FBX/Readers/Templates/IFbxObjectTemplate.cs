namespace MeshIO.FBX.Readers.Templates
{
	internal interface IFbxObjectTemplate
	{
		string TemplateId { get; }

		void Build(FbxFileBuilderBase builder, FbxPropertyTemplate properties);
	}
}
