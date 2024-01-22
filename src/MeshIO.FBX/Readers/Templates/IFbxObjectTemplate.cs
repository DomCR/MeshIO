namespace MeshIO.FBX.Readers.Templates
{
	internal interface IFbxObjectTemplate
	{
		string TemplateId { get; }

		void Build(FbxPropertyTemplate properties);
	}
}
