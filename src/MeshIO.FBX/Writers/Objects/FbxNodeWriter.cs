using MeshIO.FBX.Writers.StreamWriters;

namespace MeshIO.FBX.Writers.Objects
{
	internal class FbxNodeWriter : FbxObjectWriterBase<Node>
	{
		public override string FbxObjectName { get; } = FbxFileToken.Model;

		public override string FbxTypeName { get; } = string.Empty;

		public FbxNodeWriter(Node node) : base(node)
		{
		}

		public override void ProcessChildren(FbxFileWriterBase fwriter)
		{
			base.ProcessChildren(fwriter);

			foreach (Node node in this._element.Nodes)
			{
				fwriter.CreateConnection(node, this);
			}

			foreach (Shaders.Material mat in this._element.Materials)
			{
				fwriter.CreateConnection(mat, this);
			}

			foreach (Entities.Entity entity in this._element.Entities)
			{
				fwriter.CreateConnection(entity, this);
			}
		}

		public override void ApplyTemplate(FbxPropertyTemplate template)
		{
			base.ApplyTemplate(template);

			this.addProperty(template, "Lcl Translation", _element.Transform.Translation);
			this.addProperty(template, "Lcl Rotation", _element.Transform.EulerRotation);
			this.addProperty(template, "Lcl Scaling", _element.Transform.Scale);
		}

		protected override void writeObjectBody(FbxFileWriterBase builder, IFbxStreamWriter writer)
		{
			writer.WritePairNodeValue(FbxFileToken.Version, 232);

			base.writeObjectBody(builder, writer);

			writer.WritePairNodeValue(FbxFileToken.Shading, 'T');
			writer.WritePairNodeValue(FbxFileToken.CullingOff, "CullingOff");
		}
	}
}
