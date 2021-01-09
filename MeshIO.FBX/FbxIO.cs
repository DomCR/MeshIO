using MeshIO.FBX.IO;
using System;
using System.IO;

namespace MeshIO.FBX
{
	/// <summary>
	/// Static read and write methods.
	/// Comatible versions <see cref="FbxVersion"/>.
	/// </summary>
	public static class FbxIO
	{
		/// <summary>
		/// Reads an FBX file.
		/// </summary>
		/// <param name="path"></param>
		/// <param name="errorLevel"></param>
		/// <returns></returns>
		public static FbxRoot ReadBinaryAsRootNode(string path, ErrorLevel errorLevel = ErrorLevel.Checked)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			FbxRoot root = null;

			using (var stream = new FileStream(path, FileMode.Open))
			{
				root = new FbxBinaryReader(stream, errorLevel).Read();
			}

			return root;
		}
		/// <summary>
		/// Reads an FBX file.
		/// </summary>
		/// <param name="path"></param>
		/// <param name="errorLevel"></param>
		/// <returns></returns>
		public static FbxRoot ReadAsciiAsRootNode(string path, ErrorLevel errorLevel = ErrorLevel.Checked)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			FbxRoot root = null;

			using (var stream = new FileStream(path, FileMode.Open))
			{
				root = new FbxASCIIReader(stream, errorLevel).Read();
			}

			return root;
		}
		/// <summary>
		/// Reads an FBX file.
		/// </summary>
		/// <param name="path"></param>
		/// <param name="errorLevel"></param>
		/// <returns></returns>
		public static FbxRootDocument ReadBinary(string path, ErrorLevel errorLevel = ErrorLevel.Checked)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			FbxRootDocument rootDoc = null;

			using (var stream = new FileStream(path, FileMode.Open))
			{
				FbxRoot root = new FbxBinaryReader(stream, errorLevel).Read();
				rootDoc = new FbxRootDocument(root);
			}

			return rootDoc;
		}
		/// <summary>
		/// Reads an FBX file.
		/// </summary>
		/// <param name="path"></param>
		/// <param name="errorLevel"></param>
		/// <returns></returns>
		public static FbxRootDocument ReadAscii(string path, ErrorLevel errorLevel = ErrorLevel.Checked)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			FbxRootDocument rootDoc = null;

			using (var stream = new FileStream(path, FileMode.Open))
			{
				FbxRoot root = new FbxASCIIReader(stream, errorLevel).Read();
				rootDoc = new FbxRootDocument(root);
			}

			return rootDoc;
		}
		/// <summary>
		/// Writes an FBX file in binary.
		/// </summary>
		/// <param name="root"></param>
		/// <param name="path"></param>
		public static void WriteBinary(FbxRoot root, string path)
		{
			if (path == null)
				throw new ArgumentNullException(nameof(path));

			using (var stream = new FileStream(path, FileMode.Create))
			{
				new FbxBinaryWriter(stream).Write(root);
			}
		}
		/// <summary>
		/// Writes an FBX file in ASCII.
		/// </summary>
		/// <param name="root"></param>
		/// <param name="path"></param>
		public static void WriteAscii(FbxRoot root, string path)
		{
			if (path == null)
				throw new ArgumentNullException(nameof(path));

			using (var stream = new FileStream(path, FileMode.Create))
			{
				new FbxASCIIWriter(stream).Write(root);
			}
		}
		/// <summary>
		/// Writes an FBX file in binary.
		/// </summary>
		/// <param name="doc">This method will update the document definitions.</param>
		/// <param name="path"></param>
		public static void WriteBinary(FbxRootDocument doc, string path)
		{
			WriteBinary(doc.CreateRootNode(), path);
		}
		/// <summary>
		/// Writes an FBX file in ASCII.
		/// </summary>
		/// <param name="doc">This method will update the document definitions.</param>
		/// <param name="path"></param>
		public static void WriteAscii(FbxRootDocument doc, string path)
		{
			WriteAscii(doc.CreateRootNode(), path);
		}
	}
}
