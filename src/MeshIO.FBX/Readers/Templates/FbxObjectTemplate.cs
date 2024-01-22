using MeshIO.Entities.Geometries;
using System;

namespace MeshIO.FBX.Readers.Templates
{
	internal abstract class FbxObjectTemplate<T> : IFbxObjectTemplate
		where T : Element3D
	{
		public abstract string FbxObjectName { get; }

		public string TemplateId { get; }

		public string Prefix { get { return $"{this.FbxObjectName}::"; } }

		public FbxNode FbxNode { get; }

		public T Element { get; }

		protected FbxObjectTemplate(FbxNode node, T element)
		{
			this.FbxNode = node;
			this.Element = element;
			this.TemplateId = node.GetProperty<object>(0).ToString();
		}

		public virtual void Build(FbxFileBuilderBase builder, FbxPropertyTemplate properties)
		{
			this.Element.Id = this.FbxNode.GetProperty<ulong>(0);
			this.Element.Name = this.removePrefix(this.FbxNode.GetProperty<string>(1));

			throw new System.NotImplementedException();
		}

		protected string removePrefix(string fullname)
		{
			if (string.IsNullOrEmpty(fullname))
			{
				return string.Empty;
			}
			else if (fullname.StartsWith(this.Prefix))
			{
				return fullname.Remove(0, Prefix.Length);
			}

			return fullname;
		}
	}

	internal abstract class FbxGeometryTemplate<T> : FbxObjectTemplate<T>
		where T : Geometry
	{
		public override string FbxObjectName { get { return FbxFileToken.Geometry; } }

		protected FbxGeometryTemplate(FbxNode node, T geometry) : base(node, geometry)
		{
		}
	}

	internal class FbxMeshTemplate : FbxGeometryTemplate<Mesh>
	{
		public FbxMeshTemplate(FbxNode node) : base(node, new Mesh())
		{
		}
	}
}
