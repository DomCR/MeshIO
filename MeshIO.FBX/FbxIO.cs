using MeshIO.FBX.Converters;
using MeshIO.FBX.Readers;
using MeshIO.Elements;
using System;
using System.IO;

namespace MeshIO.FBX
{
	/// <summary>
	/// Static read and write methods
	/// </summary>
	public static class FbxIO
	{
		/// <summary>
		/// Reads a binary FBX file
		/// </summary>
		/// <param name="path"></param>
		/// <returns>The top level document node</returns>
		public static FbxRootNode ReadBinary(string path)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			using (var stream = new FileStream(path, FileMode.Open))
			{
				var reader = new FbxBinaryParser(stream);
				return reader.Parse();
			}
		}

		/// <summary>
		/// Reads an ASCII FBX file
		/// </summary>
		/// <param name="path"></param>
		/// <returns>The top level document node</returns>
		public static FbxRootNode ReadAscii(string path)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			using (var stream = new FileStream(path, FileMode.Open))
			{
				var reader = new FbxBinaryParser(stream);
				return reader.Parse();
			}
		}

		public static Scene Read(string path)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			using (FileStream stream = new FileStream(path, FileMode.Open))
			{
				FbxBinaryParser reader = new FbxBinaryParser(stream);
				var root = reader.Parse();
				IParserConverter converter = BaseParserConverter.GetConverter(root);
				return converter.ConvertScene();
			}

			throw new NotImplementedException();
		}

		/// <summary>
		/// Writes an FBX document
		/// </summary>
		/// <param name="document">The top level document node</param>
		/// <param name="path"></param>
		public static void WriteBinary(FbxRootNode document, string path)
		{
			if (path == null)
				throw new ArgumentNullException(nameof(path));
			using (var stream = new FileStream(path, FileMode.Create))
			{
				var writer = new FbxBinaryWriter(stream);
				writer.Write(document);
			}
		}

		/// <summary>
		/// Writes an FBX document
		/// </summary>
		/// <param name="document">The top level document node</param>
		/// <param name="path"></param>
		public static void WriteAscii(FbxRootNode document, string path)
		{
			if (path == null)
				throw new ArgumentNullException(nameof(path));

			using (var stream = new FileStream(path, FileMode.Create))
			{
				var writer = new FbxAsciiWriter(stream);
				writer.Write(document);
			}
		}

		/// <summary>
		/// Writes an FBX document
		/// </summary>
		/// <param name="scene"></param>
		/// <param name="path"></param>
		public static void WriteAscii(Scene scene, string path)
		{
			IFbxConverter converter = BaseFbxConverter.GetConverter(scene, FbxVersion.v7400);
			FbxRootNode root = converter.ToRootNode();

			WriteAscii(root, path);
		}
	}
}
