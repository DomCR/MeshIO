using MeshIO.Entities.Geometries;
using System;

namespace MeshIO.FBX.Readers.Templates
{
	internal abstract class FbxObjectTemplate<T> : IFbxObjectTemplate
		where T : Element3D
	{
		public string TemplateId { get; }

		public abstract string FbxObjectName { get; }

		public string Prefix { get { return $"{this.FbxObjectName}::"; } }

		public FbxNode FbxNode { get; }

		protected T _element;

		protected FbxObjectTemplate(FbxNode node)
		{
			this.FbxNode = node;

			this.TemplateId = node.GetProperty<object>(0).ToString();
		}

		public virtual void Build(FbxPropertyTemplate properties)
		{
			this._element.Id = this.FbxNode.GetProperty<ulong>(0);
			this._element.Name = this.removePrefix(this.FbxNode.GetProperty<string>(1));

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

		protected FbxGeometryTemplate(FbxNode node) : base(node)
		{
		}

		[Obsolete]
		public static IFbxObjectTemplate Create(FbxNode node)
		{
			string type = node.GetProperty<string>(2);

			switch (type)
			{
				case FbxFileToken.Mesh:
					return new FbxMeshTemplate(node);
				default:
					return null;
			}
		}
	}

	internal class FbxMeshTemplate : FbxGeometryTemplate<Mesh>
	{
		public FbxMeshTemplate(FbxNode node) : base(node)
		{
		}
	}
}
