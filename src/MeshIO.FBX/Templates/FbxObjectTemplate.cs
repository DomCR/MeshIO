using System;
using System.Collections.Generic;
using System.Diagnostics;
using MeshIO.FBX.Readers;
using MeshIO.FBX.Writers;

namespace MeshIO.FBX.Templates
{
	internal abstract class FbxObjectTemplate<T> : IFbxObjectTemplate
		where T : Element3D
	{
		public string Id { get; }

		public string Name { get { return this._element.Name; } }

		public abstract string FbxObjectName { get; }

		public abstract string FbxTypeName { get; }

		public string Prefix { get { return $"{FbxObjectName}::"; } }

		public Dictionary<string, FbxProperty> FbxInstanceProperties { get; } = new();

		public FbxNode FbxNode { get; }

		protected readonly T _element;

		protected FbxObjectTemplate(T element)
		{
			_element = element;
			Id = element.Id.ToString();
		}

		protected FbxObjectTemplate(FbxNode node, T element) : this(element)
		{
			FbxNode = node;
			Id = node?.GetProperty<object>(0).ToString();
		}

		public Element3D GetElement()
		{
			return _element;
		}

		public FbxNode ToFbxNode(FbxFileWriterBase writer)
		{
			FbxNode n = this.nodeHeader();

			this.addObjectBody(n, writer);

			return n;
		}

		public virtual void Build(FbxFileBuilderBase builder)
		{
			FbxPropertyTemplate template = builder.GetProperties(FbxObjectName);

			_element.Id = Convert.ToUInt64(FbxNode.GetProperty<long>(0));
			_element.Name = removePrefix(FbxNode.GetProperty<string>(1));

			Dictionary<string, FbxProperty> nodeProp = builder.ReadProperties(FbxNode);
			foreach (var t in template.Properties)
			{
				if (nodeProp.ContainsKey(t.Key))
				{
					continue;
				}

				nodeProp.Add(t.Key, t.Value);
			}

			addProperties(nodeProp);
		}

		public virtual void ProcessChildren(FbxFileWriterBase fbxFileWriterBase)
		{
		}

		public virtual void ApplyTemplate(FbxPropertyTemplate template)
		{
			foreach (Property item in this._element.Properties)
			{
				if (template.Properties.TryGetValue(item.Name, out FbxProperty property)
					&& item.Value == property.Value)
				{
					continue;
				}

				this.FbxInstanceProperties.Add(item.Name, FbxProperty.CreateFrom(item));
			}
		}

		protected FbxNode nodeHeader()
		{
			return new FbxNode(this.FbxObjectName, this.getId(), $"{this.FbxObjectName}::{_element.Name}", this.FbxTypeName);
		}

		protected virtual void addObjectBody(FbxNode node, FbxFileWriterBase writer)
		{
			node.Nodes.Add(writer.PropertiesToNode(this.FbxInstanceProperties.Values));
		}

		protected string removePrefix(string fullname)
		{
			if (string.IsNullOrEmpty(fullname))
			{
				return string.Empty;
			}
			else if (fullname.StartsWith(Prefix))
			{
				return fullname.Remove(0, Prefix.Length);
			}

			return fullname;
		}

		protected virtual void addProperties(Dictionary<string, FbxProperty> properties)
		{
			foreach (var prop in properties)
			{
				_element.Properties.Add(prop.Value.ToProperty());
			}
		}

		private long getId()
		{
			if (!_element.Id.HasValue)
			{
				_element.Id = IdUtils.CreateId();
			}

			return Math.Abs((long)_element.Id.Value);
		}
	}
}
