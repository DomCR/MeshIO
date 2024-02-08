using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using MeshIO.FBX.Exceptions;
using MeshIO.FBX.Writers;

namespace MeshIO.FBX
{
	/// <summary>
	/// Writes an FBX document to a binary stream
	/// </summary>
	internal class FbxBinaryWriter : FbxBinary, IFbxWriter
	{
		public FbxRootNode Root { get; }

		/// <summary>
		/// The minimum size of an array in bytes before it is compressed
		/// </summary>
		public int CompressionThreshold { get; set; } = 1024;

		private readonly Stream output;
		private readonly MemoryStream memory;
		private readonly BinaryWriter stream;

		private readonly Stack<string> nodePath = new Stack<string>();

		/// <summary>
		/// Creates a new writer
		/// </summary>
		/// <param name="root"></param>
		/// <param name="stream"></param>
		public FbxBinaryWriter(FbxRootNode root, Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException(nameof(stream));
			}

			this.Root = root;

			this.output = stream;

			// Wrap in a memory stream to guarantee seeking
			this.memory = new MemoryStream();
			this.stream = new BinaryWriter(this.memory, Encoding.ASCII);
		}

		/// <summary>
		/// Writes an FBX file to the output
		/// </summary>
		public void Write()
		{
			this.stream.BaseStream.Position = 0;
			WriteHeader(this.stream.BaseStream);
			this.stream.Write((int)this.Root.Version);
			// TODO: Do we write a top level node or not? Maybe check the version?
			this.nodePath.Clear();
			foreach (var node in this.Root.Nodes)
			{
				this.WriteNode(this.Root, node);
			}

			this.WriteNode(this.Root, null);
			this.stream.Write(GenerateFooterCode(this.Root));
			this.WriteFooter(this.stream, (int)this.Root.Version);
			this.output.Write(this.memory.GetBuffer(), 0, (int)this.memory.Position);
		}

		/// <inheritdoc/>
		public override void Dispose()
		{
			this.stream.Dispose();
			this.memory.Dispose();
			this.output.Dispose();
		}

		private delegate void PropertyWriter(BinaryWriter sw, object obj);

		struct WriterInfo
		{
			public readonly char id;
			public readonly PropertyWriter writer;

			public WriterInfo(char id, PropertyWriter writer)
			{
				this.id = id;
				this.writer = writer;
			}

			public override string ToString()
			{
				return $"{id}:{this.writer.Method}";
			}
		}

		private static readonly Dictionary<Type, WriterInfo> writePropertyActions
			= new Dictionary<Type, WriterInfo>
			{
				//{ typeof(byte), new WriterInfo('I', (sw, obj) => sw.Write((byte)obj)) },
				{ typeof(int),  new WriterInfo('I', (sw, obj) => sw.Write((int)obj)) },
				{ typeof(short),  new WriterInfo('Y', (sw, obj) => sw.Write((short)obj)) },
				{ typeof(long),   new WriterInfo('L', (sw, obj) => sw.Write((long)obj)) },
				//{ typeof(ulong),   new WriterInfo('L', (sw, obj) => sw.Write(Convert.ToInt64(obj))) },
				{ typeof(float),  new WriterInfo('F', (sw, obj) => sw.Write((float)obj)) },
				{ typeof(double), new WriterInfo('D', (sw, obj) => sw.Write((double)obj)) },
				{ typeof(char),   new WriterInfo('C', (sw, obj) => sw.Write((byte)(char)obj)) },
				{ typeof(byte[]), new WriterInfo('R', WriteRaw) },
				{ typeof(string), new WriterInfo('S', WriteString) },
				// null elements indicate arrays - they are checked again with their element type
				{ typeof(int[]),    new WriterInfo('i', null) },
				{ typeof(long[]),   new WriterInfo('l', null) },
				{ typeof(float[]),  new WriterInfo('f', null) },
				{ typeof(double[]), new WriterInfo('d', null) },
				{ typeof(bool[]),   new WriterInfo('b', null) },
			};

		static void WriteRaw(BinaryWriter stream, object obj)
		{
			var bytes = (byte[])obj;
			stream.Write(bytes.Length);
			stream.Write(bytes);
		}

		static void WriteString(BinaryWriter stream, object obj)
		{
			var str = obj.ToString();
			// Replace "::" with \0\1 and reverse the tokens
			if (str.Contains(asciiSeparator))
			{
				var tokens = str.Split(new[] { asciiSeparator }, StringSplitOptions.None);
				var sb = new StringBuilder();
				bool first = true;
				for (int i = tokens.Length - 1; i >= 0; i--)
				{
					if (!first)
					{
						sb.Append(binarySeparator);
					}

					sb.Append(tokens[i]);
					first = false;
				}
				str = sb.ToString();
			}
			var bytes = Encoding.ASCII.GetBytes(str);
			stream.Write(bytes.Length);
			stream.Write(bytes);
		}

		void WriteArray(Array array, Type elementType, PropertyWriter writer)
		{
			this.stream.Write(array.Length);

			var size = array.Length * Marshal.SizeOf(elementType);
			bool compress = size >= this.CompressionThreshold;
			this.stream.Write(compress ? 1 : 0);

			var sw = this.stream;
			DeflateWithChecksum codec = null;

			var compressLengthPos = this.stream.BaseStream.Position;
			this.stream.Write(size); // Placeholder compressed length
			var dataStart = this.stream.BaseStream.Position;
			if (compress)
			{
				this.stream.Write(new byte[] { 0x58, 0x85 }, 0, 2); // Header bytes for DeflateStream settings
				codec = new DeflateWithChecksum(this.stream.BaseStream, CompressionMode.Compress, true);
				sw = new BinaryWriter(codec);
			}
			foreach (var obj in array)
			{
				writer(sw, obj);
			}

			if (compress)
			{
				codec.Close(); // This is important - otherwise bytes can be incorrect
				var checksum = codec.Checksum;
				byte[] bytes =
				{
					(byte)((checksum >> 24) & 0xFF),
					(byte)((checksum >> 16) & 0xFF),
					(byte)((checksum >> 8) & 0xFF),
					(byte)(checksum & 0xFF),
				};
				this.stream.Write(bytes);
			}

			// Now we can write the compressed data length, since we know the size
			if (compress)
			{
				var dataEnd = this.stream.BaseStream.Position;
				this.stream.BaseStream.Position = compressLengthPos;
				this.stream.Write((int)(dataEnd - dataStart));
				this.stream.BaseStream.Position = dataEnd;
			}
		}

		void WriteProperty(object obj, int id)
		{
			if (obj == null)
			{
				return;
			}

			WriterInfo writerInfo;
			if (!writePropertyActions.TryGetValue(obj.GetType(), out writerInfo))
			{
				throw new FbxException(this.nodePath, id,
					"Invalid property type " + obj.GetType());
			}

			this.stream.Write((byte)writerInfo.id);

			if (writerInfo.writer == null) // Array type
			{
				var elementType = obj.GetType().GetElementType();
				this.WriteArray((Array)obj, elementType, writePropertyActions[elementType].writer);
			}
			else
			{
				writerInfo.writer(this.stream, obj);
			}
		}

		// Data for a null node
		static readonly byte[] nullData = new byte[13];
		static readonly byte[] nullData7500 = new byte[25];

		// Writes a single document to the buffer
		void WriteNode(FbxRootNode document, FbxNode node)
		{
			if (node == null)
			{
				var data = document.Version >= FbxVersion.v7500 ? nullData7500 : nullData;
				this.stream.BaseStream.Write(data, 0, data.Length);
			}
			else
			{
				this.nodePath.Push(node.Name ?? "");
				var name = string.IsNullOrEmpty(node.Name) ? null : Encoding.ASCII.GetBytes(node.Name);
				if (name != null && name.Length > byte.MaxValue)
				{
					throw new FbxException(this.stream.BaseStream.Position,
						"Node name is too long");
				}

				// Header
				var endOffsetPos = this.stream.BaseStream.Position;
				long propertyLengthPos;
				if (document.Version >= FbxVersion.v7500)
				{
					this.stream.Write((long)0); // End offset placeholder
					this.stream.Write((long)node.Properties.Count);
					propertyLengthPos = this.stream.BaseStream.Position;
					this.stream.Write((long)0); // Property length placeholder
				}
				else
				{
					this.stream.Write(0); // End offset placeholder
					this.stream.Write(node.Properties.Count);
					propertyLengthPos = this.stream.BaseStream.Position;
					this.stream.Write(0); // Property length placeholder
				}

				this.stream.Write((byte)(name?.Length ?? 0));
				if (name != null)
				{
					this.stream.Write(name);
				}

				// Write properties and length
				var propertyBegin = this.stream.BaseStream.Position;
				for (int i = 0; i < node.Properties.Count; i++)
				{
					this.WriteProperty(node.Properties[i], i);
				}
				var propertyEnd = this.stream.BaseStream.Position;
				this.stream.BaseStream.Position = propertyLengthPos;
				if (document.Version >= FbxVersion.v7500)
				{
					this.stream.Write((long)(propertyEnd - propertyBegin));
				}
				else
				{
					this.stream.Write((int)(propertyEnd - propertyBegin));
				}

				this.stream.BaseStream.Position = propertyEnd;

				// Write child nodes
				if (node.Nodes.Count > 0)
				{
					foreach (var n in node.Nodes)
					{
						if (n == null)
						{
							continue;
						}

						this.WriteNode(document, n);
					}
					this.WriteNode(document, null);
				}

				// Write end offset
				var dataEnd = this.stream.BaseStream.Position;
				this.stream.BaseStream.Position = endOffsetPos;
				if (document.Version >= FbxVersion.v7500)
				{
					this.stream.Write((long)dataEnd);
				}
				else
				{
					this.stream.Write((int)dataEnd);
				}

				this.stream.BaseStream.Position = dataEnd;

				this.nodePath.Pop();
			}
		}


	}
}
