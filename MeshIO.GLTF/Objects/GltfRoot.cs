using System;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO.GLTF
{
	/// <summary>
	/// The root object for a glTF asset.
	/// </summary>
	internal class GltfRoot : GltfBase
	{
		/// <summary>
		/// Names of glTF extensions used somewhere in this asset.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public List<string> extensionsUsed { get; set; } = new List<string>();
		/// <summary>
		/// Names of glTF extensions required to properly load this asset.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public List<string> extensionsRequired { get; set; } = new List<string>();
		/// <summary>
		/// An array of accessors. An accessor is a typed view into a bufferView.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public List<GltfAccessor> accessors { get; set; } = new List<GltfAccessor>();
		/// <summary>
		/// An array of keyframe animations.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public List<GltfAnimation> animations { get; set; } = new List<GltfAnimation>();
		/// <summary>
		/// Metadata about the glTF asset.
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		public GltfAsset asset { get; set; }
		/// <summary>
		/// An array of buffers. A buffer points to binary geometry, animation, or skins.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public List<GltfBuffer> buffers { get; set; } = new List<GltfBuffer>();
		/// <summary>
		/// An array of bufferViews. A bufferView is a view into a buffer generally representing a subset of the buffer.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public List<GltfBufferView> bufferViews { get; set; } = new List<GltfBufferView>();
		/// <summary>
		/// An array of cameras. A camera defines a projection matrix.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public List<GltfCamera> cameras { get; set; } = new List<GltfCamera>();
		/// <summary>
		/// An array of images. An image defines data used to create a texture.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public List<GltfImage> images { get; set; } = new List<GltfImage>();
		/// <summary>
		/// An array of materials. A material defines the appearance of a primitive.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public List<GltfMaterial> materials { get; set; } = new List<GltfMaterial>();
		/// <summary>
		/// An array of meshes. A mesh is a set of primitives to be rendered.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public List<GltfMesh> meshes { get; set; } = new List<GltfMesh>();
		/// <summary>
		/// An array of nodes.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public List<GltfNode> nodes { get; set; } = new List<GltfNode>();
		/// <summary>
		/// An array of samplers. A sampler contains properties for texture filtering and wrapping modes.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public List<GltfSampler> samplers { get; set; } = new List<GltfSampler>();
		/// <summary>
		/// The index of the default scene.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public int scene { get; set; }
		/// <summary>
		/// An array of scenes.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public List<GltfScene> scenes { get; set; } = new List<GltfScene>();
		/// <summary>
		/// An array of skins. A skin is defined by joints and matrices.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public List<GltfSkin> skins { get; set; } = new List<GltfSkin>();
		/// <summary>
		/// An array of textures.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public List<GltfTexture> textures { get; set; } = new List<GltfTexture>();
	}
}
