using CSUtilities.IO;
using MeshIO.Entities.Geometries;
using MeshIO.Formats.Gltf.Readers;
using MeshIO.Formats.Gltf.Schema.V2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO.Formats.Gltf.Builders;

internal class GltfAccessorBuilder : GltfObjectBuilder<GltfAccessor>
{
	public byte[] Bytes
	{
		get
		{
			return _bytes;
		}
	}

	public double[] Doubles
	{
		get
		{
			return _doubles;
		}
	}

	public float[] Floats
	{
		get
		{
			return _floats;
		}
	}

	public uint[] UInts
	{
		get
		{
			return _uints;
		}
	}

	public short[] Shorts
	{
		get
		{
			return _shorts;
		}
	}

	public ushort[] UShorts
	{
		get
		{
			return _uShorts;
		}
	}

	private byte[] _bytes;

	private double[] _doubles;

	private float[] _floats;

	private int[] _scalars;

	private short[] _shorts;

	private uint[] _uints;

	private ushort[] _uShorts;

	public override void Build(GlbV2FileBuilder builder)
	{
		base.Build(builder);

		StreamIO reader = builder.GetBufferStream(this.GltfObject);

		switch (GltfObject.ComponentType)
		{
			case GltfAccessor.ComponentTypeEnum.BYTE:
			case GltfAccessor.ComponentTypeEnum.UNSIGNED_BYTE:
				this.getData(this.GltfObject.Count, ref _bytes, reader.ReadByte);
				this._scalars = this._bytes.Select(b => Convert.ToInt32(b)).ToArray();
				break;
			case GltfAccessor.ComponentTypeEnum.SHORT:
				this.getData(this.GltfObject.Count, ref _shorts, reader.ReadShort);
				this._scalars = this._shorts.Select(s => Convert.ToInt32(s)).ToArray();
				break;
			case GltfAccessor.ComponentTypeEnum.UNSIGNED_SHORT:
				this.getData(this.GltfObject.Count, ref _uShorts, reader.ReadUShort);
				this._scalars = this._uShorts.Select(s => Convert.ToInt32(s)).ToArray();
				break;
			case GltfAccessor.ComponentTypeEnum.UNSIGNED_INT:
				this.getData(this.GltfObject.Count, ref _uints, reader.ReadUInt);
				this._scalars = this._uints.Select(s => Convert.ToInt32(s)).ToArray();
				break;
			case GltfAccessor.ComponentTypeEnum.FLOAT:
				this.getData(this.GltfObject.Count, ref _floats, reader.ReadSingle);
				this._doubles = this._floats.Select(f => Convert.ToDouble(f)).ToArray();
				break;
			default:
				throw new NotSupportedException();
		}
	}

	public bool TryMapTriangles(out IEnumerable<Triangle> triangles)
	{
		if (this._scalars == null || this._scalars.Length % 3 != 0)
		{
			triangles = null;
			return false;
		}

		var list = new List<Triangle>();
		for (int i = 0; i < this._scalars.Length; i++)
		{
			var i0 = this._scalars[i];
			var i1 = this._scalars[i];
			var i2 = this._scalars[i];

			list.Add(new Triangle(i0, i1, i2));
		}

		triangles = new List<Triangle>(list);
		return true;
	}

	private void getData<T>(int count, ref T[] arr, Func<T> read)
	{
		arr = new T[count];
		for (int i = 0; i < count; i++)
		{
			arr[i] = read.Invoke();
		}
	}
}
