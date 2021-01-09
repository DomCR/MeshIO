using System.Collections.Generic;

namespace MeshIO.GLTF
{
	/// <summary>
	/// Geometry to be rendered with the given material.
	/// </summary>
	internal class GltfPrimitive : GltfBase
	{
		/// <summary>
		/// A dictionary object, where each key corresponds to mesh attribute semantic and each value is the index of the accessor containing attribute's data.
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		public GlbAttribute attributes { get; set; }
		/// <summary>
		/// The index of the accessor that contains mesh indices. When this is not defined, 
		/// the primitives should be rendered without indices using drawArrays(). When defined, 
		/// the accessor must contain indices: the bufferView referenced by the accessor should 
		/// have a target equal to 34963 (ELEMENT_ARRAY_BUFFER); componentType must be 5121 
		/// (UNSIGNED_BYTE), 5123 (UNSIGNED_SHORT) or 5125 (UNSIGNED_INT), the latter may require
		/// enabling additional hardware support; type must be "SCALAR". For triangle primitives, 
		/// the front face has a counter-clockwise (CCW) winding order.
		/// Values of the index accessor must not include the maximum value for the given component 
		/// type, which triggers primitive restart in several graphics APIs and would require client
		/// implementations to rebuild the index buffer. Primitive restart values are disallowed and
		/// all index values must refer to actual vertices. As a result, the index accessor's values 
		/// must not exceed the following maxima: BYTE < 255, UNSIGNED_SHORT < 65535, UNSIGNED_INT < 4294967295.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public int? indices { get; set; }
		/// <summary>
		/// The index of the material to apply to this primitive when rendering.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public int? material { get; set; }
		/// <summary>
		/// The type of primitives to render. All valid values correspond to WebGL enums.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public GltfMappingMode mode { get; set; } = GltfMappingMode.TRIANGLES;
		/// <summary>
		/// An array of Morph Targets, each Morph Target is a dictionary mapping attributes (only POSITION, NORMAL, and TANGENT supported) 
		/// to their deviations in the Morph Target.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public List<object> targets { get; set; }
	}
}
