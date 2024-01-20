using System;
using System.Text;
using System.IO;
using System.IO.Compression;
using MeshIO.FBX.Exceptions;

namespace MeshIO.FBX.Readers.Parsers
{
	/// <summary>
	/// Reads FBX nodes from a binary stream
	/// </summary>
	internal class FbxBinaryParser : FbxBinary, IFbxParser
	{
		private delegate object ReadPrimitive(BinaryReader reader);

		private readonly BinaryReader _stream;

		private readonly ErrorLevel _errorLevel;

		/// <summary>
		/// Creates a new reader
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		/// <param name="errorLevel">When to throw an <see cref="FbxException"/></param>
		/// <exception cref="ArgumentException"><paramref name="stream"/> does
		/// not support seeking</exception>
		public FbxBinaryParser(Stream stream, ErrorLevel errorLevel = ErrorLevel.Checked)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			if (!stream.CanSeek)
				throw new ArgumentException("The stream must support seeking. Try reading the data into a buffer first");

			_stream = new BinaryReader(stream, Encoding.ASCII);
			_errorLevel = errorLevel;
		}

		/// <summary>
		/// Reads a single node.
		/// </summary>
		/// <remarks>
		/// This won't read the file header or footer, and as such will fail if the stream is a full FBX file
		/// </remarks>
		/// <returns>The node</returns>
		/// <exception cref="FbxException">The FBX data was malformed
		/// for the reader's error level</exception>
		public FbxNode ReadNode(FbxRootNode document)
		{
			var endOffset = document.Version >= FbxVersion.v7500 ? _stream.ReadInt64() : _stream.ReadInt32();
			var numProperties = document.Version >= FbxVersion.v7500 ? _stream.ReadInt64() : _stream.ReadInt32();
			var propertyListLen = document.Version >= FbxVersion.v7500 ? _stream.ReadInt64() : _stream.ReadInt32();
			var nameLen = _stream.ReadByte();
			var name = nameLen == 0 ? "" : Encoding.ASCII.GetString(_stream.ReadBytes(nameLen));

			if (endOffset == 0)
			{
				// The end offset should only be 0 in a null node
				if (_errorLevel >= ErrorLevel.Checked && (numProperties != 0 || propertyListLen != 0 || !string.IsNullOrEmpty(name)))
					throw new FbxException(_stream.BaseStream.Position,
						"Invalid node; expected NULL record");
				return null;
			}

			var node = new FbxNode { Name = name };

			var propertyEnd = _stream.BaseStream.Position + propertyListLen;
			// Read properties
			for (int i = 0; i < numProperties; i++)
				node.Properties.Add(readProperty());

			if (_errorLevel >= ErrorLevel.Checked && _stream.BaseStream.Position != propertyEnd)
				throw new FbxException(_stream.BaseStream.Position,
					"Too many bytes in property list, end point is " + propertyEnd);

			// Read nested nodes
			var listLen = endOffset - _stream.BaseStream.Position;
			if (_errorLevel >= ErrorLevel.Checked && listLen < 0)
				throw new FbxException(_stream.BaseStream.Position,
					"Node has invalid end point");
			if (listLen > 0)
			{
				FbxNode nested;
				do
				{
					nested = ReadNode(document);
					if (nested != null)
						node.Nodes.Add(nested);
				} while (nested != null);
				if (_errorLevel >= ErrorLevel.Checked && _stream.BaseStream.Position != endOffset)
					throw new FbxException(_stream.BaseStream.Position,
						"Too many bytes in node, end point is " + endOffset);
			}
			return node;
		}

		/// <summary>
		/// Reads an FBX document from the stream
		/// </summary>
		/// <returns>The top-level node</returns>
		/// <exception cref="FbxException">The FBX data was malformed
		/// for the reader's error level</exception>
		public FbxRootNode Parse()
		{
			_stream.BaseStream.Position = 0;

			// Read header
			bool validHeader = ReadHeader(_stream.BaseStream);
			if (_errorLevel >= ErrorLevel.Strict && !validHeader)
				throw new FbxException(_stream.BaseStream.Position,
					"Invalid header string");
			var document = new FbxRootNode { Version = (FbxVersion)_stream.ReadInt32() };

			// Read nodes
			var dataPos = _stream.BaseStream.Position;
			FbxNode nested;
			do
			{
				nested = ReadNode(document);
				if (nested != null)
					document.Nodes.Add(nested);
			} while (nested != null);

			// Read footer code
			var footerCode = new byte[footerCodeSize];
			_stream.BaseStream.Read(footerCode, 0, footerCode.Length);
			if (_errorLevel >= ErrorLevel.Strict)
			{
				var validCode = GenerateFooterCode(document);
				if (!CheckEqual(footerCode, validCode))
					throw new FbxException(_stream.BaseStream.Position - footerCodeSize,
						"Incorrect footer code");
			}

			// Read footer extension
			dataPos = _stream.BaseStream.Position;
			var validFooterExtension = CheckFooter(_stream, document.Version);
			if (_errorLevel >= ErrorLevel.Strict && !validFooterExtension)
				throw new FbxException(dataPos, "Invalid footer");

			return document;
		}

		/// <inheritdoc/>
		public override void Dispose()
		{
			_stream.Dispose();
		}

		// Reads a single property
		private object readProperty()
		{
			var dataType = (char)_stream.ReadByte();
			switch (dataType)
			{
				case 'Y':
					return _stream.ReadInt16();
				case 'C':
					return (char)_stream.ReadByte();
				case 'I':
					return _stream.ReadInt32();
				case 'F':
					return _stream.ReadSingle();
				case 'D':
					return _stream.ReadDouble();
				case 'L':
					return _stream.ReadInt64();
				case 'f':
					return readArray(br => br.ReadSingle(), typeof(float));
				case 'd':
					return readArray(br => br.ReadDouble(), typeof(double));
				case 'l':
					return readArray(br => br.ReadInt64(), typeof(long));
				case 'i':
					return readArray(br => br.ReadInt32(), typeof(int));
				case 'b':
					return readArray(br => br.ReadBoolean(), typeof(bool));
				case 'S':
					var len = _stream.ReadInt32();
					var str = len == 0 ? "" : Encoding.ASCII.GetString(_stream.ReadBytes(len));
					// Convert \0\1 to '::' and reverse the tokens
					if (str.Contains(binarySeparator))
					{
						var tokens = str.Split(new[] { binarySeparator }, StringSplitOptions.None);
						var sb = new StringBuilder();
						bool first = true;
						for (int i = tokens.Length - 1; i >= 0; i--)
						{
							if (!first)
								sb.Append(asciiSeparator);
							sb.Append(tokens[i]);
							first = false;
						}
						str = sb.ToString();
					}
					return str;
				case 'R':
					return _stream.ReadBytes(_stream.ReadInt32());
				default:
					throw new FbxException(_stream.BaseStream.Position - 1,
						"Invalid property data type `" + dataType + "'");
			}
		}

		// Reads an array, decompressing it if required
		private Array readArray(ReadPrimitive readPrimitive, Type arrayType)
		{
			var len = _stream.ReadInt32();
			var encoding = _stream.ReadInt32();
			var compressedLen = _stream.ReadInt32();
			var ret = Array.CreateInstance(arrayType, len);
			var stream = _stream;
			var endPos = _stream.BaseStream.Position + compressedLen;
			if (encoding != 0)
			{
				if (_errorLevel >= ErrorLevel.Checked)
				{
					if (encoding != 1)
						throw new FbxException(_stream.BaseStream.Position - 1,
							"Invalid compression encoding (must be 0 or 1)");
					var cmf = _stream.ReadByte();
					if ((cmf & 0xF) != 8 || cmf >> 4 > 7)
						throw new FbxException(_stream.BaseStream.Position - 1,
							"Invalid compression format " + cmf);
					var flg = _stream.ReadByte();
					if (_errorLevel >= ErrorLevel.Strict && ((cmf << 8) + flg) % 31 != 0)
						throw new FbxException(_stream.BaseStream.Position - 1,
							"Invalid compression FCHECK");
					if ((flg & 1 << 5) != 0)
						throw new FbxException(_stream.BaseStream.Position - 1,
							"Invalid compression flags; dictionary not supported");
				}
				else
				{
					_stream.BaseStream.Position += 2;
				}
				var codec = new DeflateWithChecksum(_stream.BaseStream, CompressionMode.Decompress);
				stream = new BinaryReader(codec);
			}
			try
			{
				for (int i = 0; i < len; i++)
					ret.SetValue(readPrimitive(stream), i);
			}
			catch (InvalidDataException)
			{
				throw new FbxException(_stream.BaseStream.Position - 1,
					"Compressed data was malformed");
			}
			if (encoding != 0)
			{
				if (_errorLevel >= ErrorLevel.Checked)
				{
					_stream.BaseStream.Position = endPos - sizeof(int);
					var checksumBytes = new byte[sizeof(int)];
					_stream.BaseStream.Read(checksumBytes, 0, checksumBytes.Length);

					int checksum = 0;
					for (int i = 0; i < checksumBytes.Length; i++)
					{
						checksum = (checksum << 8) + checksumBytes[i];
					}

#if !NET
					if (checksum != ((DeflateWithChecksum)stream.BaseStream).Checksum)
					{
						throw new FbxException(_stream.BaseStream.Position,
							"Compressed data has invalid checksum");
					}
#endif
				}
				else
				{
					_stream.BaseStream.Position = endPos;
				}
			}
			return ret;
		}
	}
}
