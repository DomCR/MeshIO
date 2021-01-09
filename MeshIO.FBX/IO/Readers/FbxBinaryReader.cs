using System;
using System.Text;
using System.IO;
using System.IO.Compression;
using MeshIO.Exceptions;

namespace MeshIO.FBX.IO
{
	/// <summary>
	/// Reads FBX nodes from a binary stream
	/// </summary>
	internal class FbxBinaryReader : FbxBinary, IDisposable
	{
		private delegate object readPrimitive(BinaryReader reader);

		private readonly BinaryReader m_stream;
		private readonly ErrorLevel m_errorLevel;
		/// <summary>
		/// Creates a new reader.
		/// </summary>
		/// <param name="stream">The stream to read from</param>
		/// <param name="errorLevel">When to throw an <see cref="FbxException"/></param>
		/// <exception cref="ArgumentException"><paramref name="stream"/> does
		/// not support seeking</exception>
		public FbxBinaryReader(Stream stream, ErrorLevel errorLevel = ErrorLevel.Checked)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			if (!stream.CanSeek)
				throw new ArgumentException(
					"The stream must support seeking. Try reading the data into a buffer first");

			this.m_stream = new BinaryReader(stream, Encoding.ASCII);
			this.m_errorLevel = errorLevel;
		}
		//**************************************************************************************
		/// <summary>
		/// Reads an FBX document from the stream
		/// </summary>
		/// <returns>The top-level node</returns>
		/// <exception cref="FbxException">The FBX data was malformed
		/// for the reader's error level</exception>
		public FbxRoot Read()
		{
			// Read header
			bool validHeader = readHeader(m_stream.BaseStream);
			if (m_errorLevel >= ErrorLevel.Strict && !validHeader)
				throw new FbxException(m_stream.BaseStream.Position, "Invalid header string");

			//Set the root node
			FbxRoot root = new FbxRoot { Version = (FbxVersion)m_stream.ReadInt32() };

			FbxNode nested;
			do
			{
				nested = ReadNode(root);
				if (nested != null)
					root.Nodes.Add(nested);
			} while (nested != null);

			// Read footer code
			var footerCode = new byte[footerCodeSize];
			m_stream.BaseStream.Read(footerCode, 0, footerCode.Length);
			if (m_errorLevel >= ErrorLevel.Strict)
			{
				var validCode = generateFooterCode(root);
				if (!checkEqual(footerCode, validCode))
					throw new FbxException(m_stream.BaseStream.Position - footerCodeSize,
						"Incorrect footer code");
			}

			// Read footer extension
			long dataPos = m_stream.BaseStream.Position;
			var validFooterExtension = checkFooter(m_stream, root.Version);
			if (m_errorLevel >= ErrorLevel.Strict && !validFooterExtension)
				throw new FbxException(dataPos, "Invalid footer");
			return root;
		}
		/// <summary>
		/// Reads a single node.
		/// </summary>
		/// <remarks>
		/// This won't read the file header or footer, and as such will fail if the stream is a full FBX file.
		/// </remarks>
		/// <returns>The node</returns>
		/// <exception cref="FbxException">The FBX data was malformed
		/// for the reader's error level</exception>
		public FbxNode ReadNode(FbxRoot root)
		{
			long endOffset = root.Version >= FbxVersion.v7500 ? m_stream.ReadInt64() : m_stream.ReadInt32();
			long numProperties = root.Version >= FbxVersion.v7500 ? m_stream.ReadInt64() : m_stream.ReadInt32();
			long propertyListLen = root.Version >= FbxVersion.v7500 ? m_stream.ReadInt64() : m_stream.ReadInt32();
			byte nameLen = m_stream.ReadByte();
			string name = nameLen == 0 ? "" : Encoding.ASCII.GetString(m_stream.ReadBytes(nameLen));

			if (endOffset == 0)
			{
				// The end offset should only be 0 in a null node
				if (m_errorLevel >= ErrorLevel.Checked && (numProperties != 0 || propertyListLen != 0 || !string.IsNullOrEmpty(name)))
					throw new FbxException(m_stream.BaseStream.Position, "Invalid node; expected NULL record");
				return null;
			}

			var node = new FbxNode { Name = name };

			var propertyEnd = m_stream.BaseStream.Position + propertyListLen;
			// Read properties
			for (int i = 0; i < numProperties; i++)
				node.Properties.Add(readProperty());

			if (m_errorLevel >= ErrorLevel.Checked && m_stream.BaseStream.Position != propertyEnd)
				throw new FbxException(m_stream.BaseStream.Position, "Too many bytes in property list, end point is " + propertyEnd);

			// Read nested nodes
			var listLen = endOffset - m_stream.BaseStream.Position;
			if (m_errorLevel >= ErrorLevel.Checked && listLen < 0)
				throw new FbxException(m_stream.BaseStream.Position, "Node has invalid end point");
			if (listLen > 0)
			{
				FbxNode nested;
				do
				{
					nested = ReadNode(root);
					node.Nodes.Add(nested);
				} while (nested != null);
				if (m_errorLevel >= ErrorLevel.Checked && m_stream.BaseStream.Position != endOffset)
					throw new FbxException(m_stream.BaseStream.Position, "Too many bytes in node, end point is " + endOffset);
			}
			return node;
		}
		/// <inheritdoc/>
		public void Dispose()
		{
			this.m_stream.Dispose();
		}
		//**************************************************************************************
		/// <summary> 
		/// Reads a single property
		/// </summary>
		/// <returns></returns>
		private object readProperty()
		{
			var dataType = (char)m_stream.ReadByte();
			switch (dataType)
			{
				case 'Y':
					return m_stream.ReadInt16();
				case 'C':
					return (char)m_stream.ReadByte();
				case 'I':
					return m_stream.ReadInt32();
				case 'F':
					return m_stream.ReadSingle();
				case 'D':
					return m_stream.ReadDouble();
				case 'L':
					return m_stream.ReadInt64();
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
					var len = m_stream.ReadInt32();
					var str = len == 0 ? "" : Encoding.ASCII.GetString(m_stream.ReadBytes(len));
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
					return m_stream.ReadBytes(m_stream.ReadInt32());
				default:
					throw new FbxException(m_stream.BaseStream.Position - 1, "Invalid property data type `" + dataType + "'");
			}
		}
		/// <summary>
		/// Reads an array, decompressing it if required
		/// </summary>
		/// <param name="readPrimitive"></param>
		/// <param name="arrayType"></param>
		/// <returns></returns>
		private Array readArray(readPrimitive readPrimitive, Type arrayType)
		{
			var len = m_stream.ReadInt32();
			var encoding = m_stream.ReadInt32();
			var compressedLen = m_stream.ReadInt32();
			var ret = Array.CreateInstance(arrayType, len);
			var s = m_stream;
			var endPos = m_stream.BaseStream.Position + compressedLen;
			if (encoding != 0)
			{
				if (m_errorLevel >= ErrorLevel.Checked)
				{
					if (encoding != 1)
						throw new FbxException(m_stream.BaseStream.Position - 1, "Invalid compression encoding (must be 0 or 1)");
					var cmf = m_stream.ReadByte();
					if ((cmf & 0xF) != 8 || (cmf >> 4) > 7)
						throw new FbxException(m_stream.BaseStream.Position - 1, "Invalid compression format " + cmf);
					var flg = m_stream.ReadByte();
					if (m_errorLevel >= ErrorLevel.Strict && ((cmf << 8) + flg) % 31 != 0)
						throw new FbxException(m_stream.BaseStream.Position - 1, "Invalid compression FCHECK");
					if ((flg & (1 << 5)) != 0)
						throw new FbxException(m_stream.BaseStream.Position - 1, "Invalid compression flags; dictionary not supported");
				}
				else
				{
					m_stream.BaseStream.Position += 2;
				}
				var codec = new DeflateWithChecksum(m_stream.BaseStream, CompressionMode.Decompress);
				s = new BinaryReader(codec);
			}
			try
			{
				for (int i = 0; i < len; i++)
					ret.SetValue(readPrimitive(s), i);
			}
			catch (InvalidDataException)
			{
				throw new FbxException(m_stream.BaseStream.Position - 1,
					"Compressed data was malformed");
			}
			if (encoding != 0)
			{
				if (m_errorLevel >= ErrorLevel.Checked)
				{
					m_stream.BaseStream.Position = endPos - sizeof(int);
					var checksumBytes = new byte[sizeof(int)];
					m_stream.BaseStream.Read(checksumBytes, 0, checksumBytes.Length);
					int checksum = 0;
					for (int i = 0; i < checksumBytes.Length; i++)
						checksum = (checksum << 8) + checksumBytes[i];
					if (checksum != ((DeflateWithChecksum)s.BaseStream).Checksum)
						throw new FbxException(m_stream.BaseStream.Position, "Compressed data has invalid checksum");
				}
				else
				{
					m_stream.BaseStream.Position = endPos;
				}
			}
			return ret;
		}
	}
}
