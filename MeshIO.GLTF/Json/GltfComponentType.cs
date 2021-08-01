using System;

namespace MeshIO.GLTF
{
	internal enum GltfComponentType
	{
		/// <summary>
		/// <see cref="System.SByte"/>, size in bytes 1.
		/// </summary>
		BYTE = 5120,
		/// <summary>
		/// <see cref="System.Byte"/>, size in bytes 1.
		/// </summary>
		UNSIGNED_BYTE = 5121,
		/// <summary>
		/// <see cref="System.Int16"/>, size in bytes 2.
		/// </summary>
		SHORT = 5122,
		/// <summary>
		/// <see cref="System.UInt16"/>, size in bytes 2.
		/// </summary>
		UNSIGNED_SHORT = 5123,
		/// <summary>
		/// <see cref="System.Int32"/>, size in bytes 4.
		/// </summary>
		UNSIGNED_INT = 5125,
		/// <summary>
		/// <see cref="System.Single"/>, size in bytes 4.
		/// </summary>
		FLOAT = 5126,
	}

	internal static class GltfComponentTypeEstensions
	{
		public static int GetMemorySize(this GltfComponentType gltfComponent)
		{
			switch (gltfComponent)
			{
				case GltfComponentType.BYTE:
				case GltfComponentType.UNSIGNED_BYTE:
					return 1;
				case GltfComponentType.SHORT:
				case GltfComponentType.UNSIGNED_SHORT:
					return 2;
				case GltfComponentType.UNSIGNED_INT:
					return 4;
				case GltfComponentType.FLOAT:
					return 4;
				default:
					return 0;
			}
		}
		public static Type GetValueType(this GltfComponentType gltfComponent)
		{
			switch (gltfComponent)
			{
				case GltfComponentType.BYTE:
					return typeof(sbyte);
				case GltfComponentType.UNSIGNED_BYTE:
					return typeof(byte);
				case GltfComponentType.SHORT:
					return typeof(short);
				case GltfComponentType.UNSIGNED_SHORT:
					return typeof(ushort);
				case GltfComponentType.UNSIGNED_INT:
					return typeof(uint);
				case GltfComponentType.FLOAT:
					return typeof(float);
				default:
					return null;
			}
		}
	}
}
