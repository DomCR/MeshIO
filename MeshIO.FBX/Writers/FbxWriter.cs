using MeshIO.Elements;
using MeshIO.FBX.Converters;
using System;
using System.IO;

namespace MeshIO.FBX
{
	public class FbxWriter : IFbxWriter
	{
		public string Path { get; set; }
		FbxVersion Version { get; set; }
		public Scene Scene { get; set; }

		public FbxWriter(string path, Scene scene, FbxVersion version = FbxVersion.v7400)
		{
			this.Path = path;
			this.Scene = scene;
			this.Version = version;
		}

		public FbxRootNode GetRootNode()
		{
			IFbxConverter converter = FbxConverterBase.GetConverter(Scene, Version);
			return converter.ToRootNode();
		}

		public FbxRootNode GetRootNode(FbxVersion version)
		{
			IFbxConverter converter = FbxConverterBase.GetConverter(Scene, version);
			return converter.ToRootNode();
		}

		public void WriteAscii()
		{
			if (Path == null)
				throw new ArgumentNullException(nameof(Path));

			using (FileStream stream = new FileStream(Path, FileMode.Create))
			{
				using (FbxAsciiWriter writer = new FbxAsciiWriter(stream))
					writer.Write(GetRootNode());
			}
		}

		public void WriteBinary()
		{
			if (Path == null)
				throw new ArgumentNullException(nameof(Path));

			using (FileStream stream = new FileStream(Path, FileMode.Create))
			{
				using (FbxBinaryWriter writer = new FbxBinaryWriter(stream))
					writer.Write(GetRootNode());
			}
		}

		public static void WriteAscii(string path, Scene scene, FbxVersion version = FbxVersion.v7400)
		{
			FbxWriter writer = new FbxWriter(path, scene, version);
			writer.WriteAscii();
		}

		public static void WriteAscii(string path, FbxRootNode root)
		{
			if (path == null)
				throw new ArgumentNullException(nameof(path));

			using (FileStream stream = new FileStream(path, FileMode.Create))
			{
				using (FbxAsciiWriter writer = new FbxAsciiWriter(stream))
					writer.Write(root);
			}
		}

		public static void WriteBinary(string path, Scene scene, FbxVersion version = FbxVersion.v7400)
		{
			FbxWriter writer = new FbxWriter(path, scene, version);
			writer.WriteBinary();
		}

		public static void WriteBinary(string path, FbxRootNode root)
		{
			if (path == null)
				throw new ArgumentNullException(nameof(path));

			using (FileStream stream = new FileStream(path, FileMode.Create))
			{
				using (FbxBinaryWriter writer = new FbxBinaryWriter(stream))
					writer.Write(root);
			}
		}
	}
}
