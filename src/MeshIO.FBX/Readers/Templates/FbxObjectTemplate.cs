using System;
using System.Collections.Generic;

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

		protected FbxObjectTemplate(T element)
		{
			this.Element = element;
			this.TemplateId = element.Id.ToString();
		}

		protected FbxObjectTemplate(FbxNode node, T element)
		{
			this.FbxNode = node;
			this.Element = element;
			this.TemplateId = node?.GetProperty<object>(0).ToString();
		}

		public Element3D GetElement()
		{
			return this.Element;
		}

		public virtual void Build(FbxFileBuilderBase builder)
		{
			FbxPropertyTemplate template = builder.GetProperties(this.FbxObjectName);

			this.Element.Id = Convert.ToUInt64(this.FbxNode.GetProperty<long>(0));
			this.Element.Name = this.removePrefix(this.FbxNode.GetProperty<string>(1));

			Dictionary<string, FbxProperty> nodeProp = builder.ReadProperties(FbxNode);
			foreach (var t in template.Properties)
			{
				if (nodeProp.ContainsKey(t.Key))
				{
					continue;
				}

				nodeProp.Add(t.Key, t.Value);
			}
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

		protected virtual void addProperties(Dictionary<string, FbxProperty> properties)
		{
			foreach (var prop in properties)
			{
				this.Element.Properties.Add(prop.Value.ToProperty());
			}
		}
	}
}
