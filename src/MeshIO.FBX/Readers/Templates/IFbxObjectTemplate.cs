namespace MeshIO.FBX.Readers.Templates
{
	internal interface IFbxObjectTemplate
	{
		string TemplateId { get; }

		Element3D GetElement();

		void Build(FbxFileBuilderBase builder);
	}
}
