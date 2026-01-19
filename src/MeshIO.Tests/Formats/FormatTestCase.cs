using MeshIO.Formats;
using System.IO;
using Xunit.Abstractions;

namespace MeshIO.Tests.Formats
{
	public class FormatTestCase : IXunitSerializable
	{
		public ContentType Content { get; set; }

		public string Extension
		{
			get
			{
				return FileFormat.GetExtension(this.Format);
			}
		}

		public FileFormatType Format { get; set; }

		public string Name { get; set; }

		public string Path
		{
			get
			{
				string path = System.IO.Path.Combine(TestVariables.OutputSamplesFolder, this.Extension, $"{Name}_{Format}_{Content}.{Extension}");
				System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
				return path;
			}
		}

		public FormatTestCase()
		{ }

		public FormatTestCase(string name, FileFormatType format, ContentType content)
		{
			this.Name = name;
			this.Format = format;
			this.Content = content;
		}

		public void Deserialize(IXunitSerializationInfo info)
		{
			this.Name = info.GetValue<string>(nameof(this.Name));
			this.Format = (FileFormatType)info.GetValue<int>(nameof(this.Format));
			this.Content = (ContentType)info.GetValue<int>(nameof(this.Content));
		}

		public void Serialize(IXunitSerializationInfo info)
		{
			info.AddValue(nameof(this.Name), this.Name);
			info.AddValue(nameof(this.Format), (int)this.Format);
			info.AddValue(nameof(this.Content), (int)this.Content);
		}

		public override string ToString()
		{
			return $"{Format} {Content}";
		}
	}
}