#if NETFRAMEWORK
using CSUtilities.Extensions;
#endif

using MeshIO.Entities;
using MeshIO.Formats.Fbx.Writers;
using MeshIO.Shaders;

namespace MeshIO.Formats.Fbx.Templates;

internal class FbxNodeTemplate : FbxObjectTemplate<Node>
{
	public override string FbxObjectName { get { return FbxFileToken.Model; } }

	public override string FbxTypeName { get { return FbxFileToken.Mesh; } }

	public FbxNodeTemplate(FbxVersion version,Node root) : base(version,root)
	{
	}

	public override void ProcessChildren(FbxFileWriterBase writer)
	{
		base.ProcessChildren(writer);

		foreach (Node node in this._element.Nodes)
		{
			writer.CreateConnection(node, this);
		}

		foreach (Material mat in this._element.Materials)
		{
			writer.CreateConnection(mat, this);
		}

		foreach (Entity entity in this._element.Entities)
		{
			writer.CreateConnection(entity, this);
		}
	}

	protected override void addObjectBody(FbxNode node, FbxFileWriterBase writer)
	{
		node.Add(FbxFileToken.Version, 232);

		base.addObjectBody(node, writer);

		node.Add(FbxFileToken.Shading, 'T');
		node.Add(FbxFileToken.CullingOff, "CullingOff");
	}
}