using MeshIO.FBX.Writers.StreamWriters;
using System;
using System.Collections.Generic;

namespace MeshIO.FBX.Writers.Objects
{
	internal abstract class FbxObjectWriterBase<T> : IFbxObjectWriter
		where T : Element3D
	{
		public ulong Id { get { return this._element.Id.Value; } }

		public string Name { get { return this._element.Name; } }

		public abstract string FbxObjectName { get; }

		public abstract string FbxTypeName { get; }

		public Dictionary<string, FbxProperty> FbxInstanceProperties { get; } = new();

		protected readonly T _element;

		protected FbxObjectWriterBase(T element)
		{
			_element = element;
		}

		public void Write(FbxFileWriterBase builder, IFbxStreamWriter writer)
		{
			this.writeObjectHeader(writer);

			writer.WriteOpenBracket();

			this.writeObjectBody(builder, writer);

			writer.WriteCloseBracket();
			writer.WriteEmptyLine();
		}

		public virtual void ProcessChildren(FbxFileWriterBase fbxFileWriterBase)
		{
		}

		public virtual void ApplyTemplate(FbxPropertyTemplate template)
		{
			foreach (Property item in this._element.Properties)
			{
				if (template.Properties.TryGetValue(item.Name, out FbxProperty property)
					&& item.Value.Equals(property.Value))
				{
					continue;
				}

				this.FbxInstanceProperties.Add(item.Name, FbxProperty.CreateFrom(item));
			}
		}

		protected virtual void writeObjectHeader(IFbxStreamWriter writer)
		{
			writer.WriteName(this.FbxObjectName);
			writer.WriteValue(this.getId());
			writer.WriteValue($"{this.FbxObjectName}::{_element.Name}");
			writer.WriteValue(this.FbxTypeName);
		}

		protected virtual void writeObjectBody(FbxFileWriterBase builder, IFbxStreamWriter writer)
		{
			builder.WriteProperties(this.FbxInstanceProperties.Values);
		}

		protected void addProperty(FbxPropertyTemplate template, string name, object value)
		{
			if (template.GetUpdated(name, value, out FbxProperty p))
			{
				this.FbxInstanceProperties.Add(p.Name, p);
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
