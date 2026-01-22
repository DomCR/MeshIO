using MeshIO.Formats;
using MeshIO.Formats.Fbx;
using Xunit.Abstractions;

namespace MeshIO.Tests.TestModels;

public class FbxFileModel : FileModel
{
	public ContentType Content { get; private set; }

	public FbxVersion Version { get; private set; }

	public FbxFileModel()
				: base()
	{
	}

	public FbxFileModel(string path)
		: base(path)
	{
	}

	public FbxFileModel(string folder, FbxVersion version, ContentType content)
		: this(System.IO.Path.Combine(folder, $"{version}_{content}.fbx"))
	{
		this.Version = version;
		this.Content = content;
	}

	public override void Deserialize(IXunitSerializationInfo info)
	{
		base.Deserialize(info);

		this.Content = info.GetValue<ContentType>(nameof(this.Content));
		this.Version = info.GetValue<FbxVersion>(nameof(this.Version));
	}

	public override void Serialize(IXunitSerializationInfo info)
	{
		base.Serialize(info);

		info.AddValue(nameof(this.Version), this.Version);
		info.AddValue(nameof(this.Content), this.Content);
	}
}