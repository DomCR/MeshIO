using MeshIO.Formats.Gltf.Schema.V2;
using MeshIO.Materials;

namespace MeshIO.Formats.Gltf.Schema;

internal static class GltfConverter
{
	public static WrapMode Convert(this int wrapMode)
	{
		switch (wrapMode)
		{
			case 10497:
				return WrapMode.Repeat;
			case 33648:
				return WrapMode.MirroredRepeat;
			case 33071:
			default:
				return WrapMode.Clamp;
		}
	}

	public static TextureFilterType Convert(this int? filter)
	{
		return filter.Convert(out _);
	}

	public static TextureFilterType Convert(this int? filter, out TextureFilterType mipFilter)
	{
		mipFilter = TextureFilterType.None;
		switch (filter)
		{
			case 9728: // NEAREST
				return TextureFilterType.Nearest;
			case 9729: // LINEAR
				return TextureFilterType.Linear;
			case 9984: // NEAREST_MIPMAP_NEAREST
				mipFilter = TextureFilterType.Nearest;
				return TextureFilterType.Nearest;
			case 9985: // LINEAR_MIPMAP_NEAREST
				mipFilter = TextureFilterType.Nearest;
				return TextureFilterType.Linear;
			case 9986: // NEAREST_MIPMAP_LINEAR
				mipFilter = TextureFilterType.Linear;
				return TextureFilterType.Nearest;
			case 9987: // LINEAR_MIPMAP_LINEAR
				mipFilter = TextureFilterType.Linear;
				return TextureFilterType.Linear;
			default:
				return TextureFilterType.Nearest;
		}
	}
}