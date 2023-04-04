﻿using MeshIO.Core;
using System;
using System.IO;

namespace MeshIO.FBX
{
	/// <summary>
	/// Implements a <see cref="FbxWriter"/> for writing fbx files.
	/// </summary>
	public class FbxWriter : WriterBase
	{
		/// <summary>
		/// Version to be used in the fbx file.
		/// </summary>
		public FbxVersion Version { get; }

		public Scene Scene { get; }

		public FbxRootNode RootNode { get; }

		private Stream _stream;

		/// <summary>
		/// Initializes a new instance of the <see cref="FbxWriter"/> class for the specified file.
		/// </summary>
		/// <param name="path">The complete file path to write to.</param>
		/// <param name="scene"></param>
		/// <param name="version"></param>
		public FbxWriter(string path, Scene scene, FbxVersion version = FbxVersion.v7400) : this(File.Create(path), scene, version) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="FbxWriter"/> class for the specified stream.
		/// </summary>
		/// <param name="stream">The stream to write to.</param>
		/// <param name="scene"></param>
		/// <param name="version"></param>
		public FbxWriter(Stream stream, Scene scene, FbxVersion version = FbxVersion.v7400)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			if (!stream.CanSeek)
				throw new ArgumentException("The stream must support seeking. Try reading the data into a buffer first");

			this.Scene = scene;
			this.Version = version;
			this._stream = stream;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FbxWriter"/> class for the specified file.
		/// </summary>
		/// <param name="path">The complete file path to write to.</param>
		/// <param name="rootNode"></param>
		/// <param name="version"></param>
		public FbxWriter(string path, FbxRootNode rootNode, FbxVersion version = FbxVersion.v7400) : this(File.Create(path), rootNode, version) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="FbxWriter"/> class for the specified stream.
		/// </summary>
		/// <param name="stream">The stream to write to.</param>
		/// <param name="rootNode"></param>
		/// <param name="version"></param>
		public FbxWriter(Stream stream, FbxRootNode rootNode, FbxVersion version = FbxVersion.v7400)
		{
			this.Version = version;
			this.RootNode = rootNode;
			this._stream = stream;
		}

		/// <summary>
		/// Write a <see cref="Scene"/> into a fbx an ascii file.
		/// </summary>
		public void WriteAscii(Scene scene)
		{
			using (FbxAsciiWriter writer = new FbxAsciiWriter(this._stream))
				writer.Write(FbxRootNode.CreateFromScene(scene, this.Version));
		}

		/// <summary>
		/// Write an <see cref="Scene"/> into an fbx binary file.
		/// </summary>
		public void WriteBinary(Scene scene)
		{
			using (FbxBinaryWriter writer = new FbxBinaryWriter(this._stream))
				writer.Write(FbxRootNode.CreateFromScene(scene, this.Version));
		}

		/// <summary>
		/// Write a <see cref="FbxRootNode"/> into a fbx an ascii file.
		/// </summary>
		public void WriteAscii(FbxRootNode root)
		{
			using (FbxAsciiWriter writer = new FbxAsciiWriter(this._stream))
				writer.Write(root);
		}

		/// <summary>
		/// Write a <see cref="FbxRootNode"/> into a fbx an ascii file.
		/// </summary>
		public void WriteBinary(FbxRootNode root)
		{
			using (FbxBinaryWriter writer = new FbxBinaryWriter(this._stream))
				writer.Write(root);
		}

		/// <inheritdoc/>
		public override void Dispose()
		{
			_stream.Dispose();
		}

		public static void WriteAscii(string path, Scene scene, FbxVersion version = FbxVersion.v7400)
		{
			using (FbxWriter writer = new FbxWriter(path, scene, version))
				writer.WriteAscii(scene);
		}

		public static void WriteAscii(string path, FbxRootNode root)
		{
			using (FbxWriter writer = new FbxWriter(path, root, root.Version))
				writer.WriteAscii(root);
		}

		public static void WriteBinary(string path, Scene scene, FbxVersion version = FbxVersion.v7400)
		{
			if (path == null)
				throw new ArgumentNullException(nameof(path));

			using (FbxWriter writer = new FbxWriter(path, scene, version))
				writer.WriteBinary(scene);
		}

		public static void WriteBinary(string path, FbxRootNode root)
		{
			using (FbxWriter writer = new FbxWriter(path, root, root.Version))
				writer.WriteBinary(root);
		}
	}
}