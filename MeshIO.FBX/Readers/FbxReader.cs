﻿using MeshIO.Elements;
using MeshIO.FBX.Converters;
using MeshIO.FBX.Exceptions;
using System;
using System.IO;

namespace MeshIO.FBX
{
	public class FbxReader : IFbxReader
	{
		private Stream _stream;
		private ErrorLevel _errorLevel;

		/// <summary>
		/// Initializes a new instance of the <see cref="FbxReader"/> class for the specified file.
		/// </summary>
		/// <param name="path">The complete file path to read to.</param>
		/// <param name="errorLevel">When to throw an <see cref="FbxException"/></param>
		public FbxReader(string path, ErrorLevel errorLevel)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			_stream = new FileStream(path, FileMode.Open);
			_errorLevel = errorLevel;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FbxReader"/> class for the specified stream.
		/// </summary>
		/// <param name="stream">The stream to write to.</param>
		/// <param name="errorLevel"></param>
		public FbxReader(Stream stream, ErrorLevel errorLevel)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			if (!stream.CanSeek)
				throw new ArgumentException("The stream must support seeking. Try reading the data into a buffer first");

			_stream = stream;
			_errorLevel = errorLevel;
		}

		/// <summary>
		/// Read the file into an fbx scene.
		/// </summary>
		/// <returns></returns>
		public Scene Read()
		{
			FbxRootNode root = this.Parse();
			INodeParser converter = NodeParserBase.GetConverter(root);

			return converter.ConvertScene();
		}

		/// <summary>
		/// Parse the document into a node structure.
		/// </summary>
		/// <returns></returns>
		public FbxRootNode Parse()
		{
			IFbxParser parser = null;

			if (FbxBinary.ReadHeader(_stream))
			{
				parser = new FbxBinaryParser(_stream, _errorLevel);
			}
			else
			{
				parser = new FbxAsciiParser(_stream, _errorLevel);
			}

			return parser.Parse();
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			_stream.Dispose();
		}

		/// <summary>
		/// Read fbx file.
		/// </summary>
		/// <returns></returns>
		public static Scene Read(string path, ErrorLevel errorLevel)
		{
			using (FbxReader reader = new FbxReader(path, errorLevel))
			{
				return reader.Read();
			}
		}

		/// <summary>
		/// Parse the document into a node structure.
		/// </summary>
		/// <returns></returns>
		public static FbxRootNode Parse(string path, ErrorLevel errorLevel)
		{
			using (FbxReader reader = new FbxReader(path, errorLevel))
			{
				return reader.Parse();
			}
		}

		public static Scene Read(Stream stream, ErrorLevel errorLevel)
		{
			using (FbxReader reader = new FbxReader(stream, errorLevel))
			{
				return reader.Read();
			}
		}

		public static FbxRootNode Parse(Stream stream, ErrorLevel errorLevel)
		{
			using (FbxReader reader = new FbxReader(stream, errorLevel))
			{
				return reader.Parse();
			}
		}
	}
}
