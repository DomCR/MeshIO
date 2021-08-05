using CSUtilities.Converters;
using CSUtilities.IO;
using MeshIO.Elements;
using MeshIO.Elements.Geometries;
using MeshIO.GLTF.Exceptions;
using MeshIO.GLTF.Schema.V2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.GLTF
{
	internal abstract class GltfBinaryReaderBase : IDisposable
	{
		public static GltfBinaryReaderBase GetBynaryReader(int version, GltfRoot root, byte[] chunk)
		{
			switch (version)
			{
				case 2:
					return new GltfBinaryReaderV2(root, chunk);
				case 1:
				default:
					throw new NotImplementedException($"Version {version} not implemented");
			}
		}

		protected readonly Dictionary<int, List<Mesh>> _meshMap = new Dictionary<int, List<Mesh>>();
		protected readonly GltfRoot _root;
		protected byte[] _chunk;

		public GltfBinaryReaderBase(GltfRoot root, byte[] chunk)
		{
			this._root = root;
			this._chunk = chunk;
		}

		public Scene Read()
		{
			GltfScene rootScene = this._root.Scenes[this._root.Scene.Value];
			Scene scene = new Scene(rootScene.Name);

			foreach (int nodeIndex in rootScene.Nodes)
			{
				Node a = readNode(nodeIndex);
			}

			throw new NotImplementedException();
		}

		public void Dispose()
		{

		}

		protected Node readNode(int index)
		{
			GltfNode gltfNode = this._root.Nodes[index];
			Node fbxNode = new Node(gltfNode.Name);

			if (gltfNode.Camera.HasValue)
				fbxNode.Children.Add(readCamera(gltfNode.Camera.Value));

			foreach (int i in gltfNode.Children)
			{

			}

			if (gltfNode.Skin.HasValue)
			{
				//TODO: read skin
			}

			if (gltfNode.Matrix != null)
			{
				//TODO: Apply transform matrix
			}

			if (gltfNode.Mesh.HasValue)
			{
				fbxNode.Children.AddRange(readPrimitivesInMesh(gltfNode.Mesh.Value));
			}

			throw new NotImplementedException();
		}

		protected Camera readCamera(int index)
		{
			GltfCamera gltfCamera = this._root.Cameras[index];
			Camera fbxCamera = new Camera(gltfCamera.Name);

			//TODO: implement gltf camera reader
			switch (gltfCamera.Type)
			{
				case GltfCamera.TypeEnum.perspective:
					break;
				case GltfCamera.TypeEnum.orthographic:
					break;
				default:
					throw new Exception();
			}

			throw new NotImplementedException();
		}

		protected Element readElement(int index)
		{
			throw new NotImplementedException();
		}

		protected List<Mesh> readPrimitivesInMesh(int index)
		{
			GltfMesh gltfMesh = this._root.Meshes[index];
			List<Mesh> meshes = new List<Mesh>();

			foreach (GltfMeshPrimitive p in gltfMesh.Primitives)
			{

			}

			throw new NotImplementedException();
		}
	}

	internal class GltfBinaryReaderV2 : GltfBinaryReaderBase
	{
		public GltfBinaryReaderV2(GltfRoot root, byte[] chunk) : base(root, chunk) { }
	}

	public class GltfReader : IDisposable
	{
		private GlbHeader _header;
		private GltfRoot _root;
		private readonly StreamIO _stream;
		private StreamIO _binaryStream;

		public GltfReader(string path)
		{
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			_stream = new StreamIO(new FileStream(path, FileMode.Open));
		}

		public GltfReader(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			if (!stream.CanSeek)
				throw new ArgumentException("The stream must support seeking. Try reading the data into a buffer first");

			_stream = new StreamIO(stream);
		}

		public void Read()
		{
			//The 12-byte header consists of three 4-byte entries:
			_header = new GlbHeader();
			//magic equals 0x46546C67. It is ASCII string glTF, and can be used to identify data as Binary glTF.
			_header.Magic = _stream.ReadUInt<LittleEndianConverter>();
			//version indicates the version of the Binary glTF container format. This specification defines version 2.
			_header.Version = _stream.ReadUInt<LittleEndianConverter>();
			//length is the total length of the Binary glTF, including Header and all Chunks, in bytes.
			_header.Length = _stream.ReadUInt<LittleEndianConverter>();

			if (_header.Version != 2)
				throw new NotImplementedException($"Version {_header.Version} not implemented");

			//Chunk 0 Json
			uint jsonChunkLength = _stream.ReadUInt<LittleEndianConverter>();
			string jsonChunkType = _stream.ReadString(4);

			if (jsonChunkType != "JSON")
				throw new GltfReaderException("Chunk type does not match", _stream.Position);

			_root = JsonConvert.DeserializeObject<GltfRoot>(_stream.ReadString((int)jsonChunkLength));

			//Chunk 1 bin
			uint binChunkLength = _stream.ReadUInt<LittleEndianConverter>();
			string binChunkType = _stream.ReadString(4);

			//Check the chunk type
			if (binChunkType != "BIN\0")
				throw new GltfReaderException("Chunk type does not match", _stream.Position);

			byte[] binChunk = _stream.ReadBytes((int)binChunkLength);
			_binaryStream = new StreamIO(binChunk);

			GltfBinaryReaderBase.GetBynaryReader((int)_header.Version, _root, binChunk).Read();
		}

		public void Dispose()
		{
			_stream.Dispose();
			_binaryStream.Dispose();
		}
	}

	internal class GlbHeader
	{
		public uint Magic { get; set; }
		public uint Version { get; set; }
		public uint Length { get; set; }
	}
}
