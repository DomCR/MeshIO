using MeshIO.Elements;
using MeshIO.FBX.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.FBX
{
	public class FbxReader : IFbxReader
	{
		private Stream _stream;
		private ErrorLevel _errorLevel;

		public FbxReader(string path, ErrorLevel errorLevel)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			_stream = new FileStream(path, FileMode.Open);
			_errorLevel = errorLevel;
		}

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

		/// <summary>
		/// Read fbx file.
		/// </summary>
		/// <param name="root"></param>
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

		/// <inheritdoc/>
		public void Dispose()
		{
			_stream.Dispose();
		}
	}
}
