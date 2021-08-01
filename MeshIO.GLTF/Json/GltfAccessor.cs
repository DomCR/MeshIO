using System.Collections.Generic;

namespace MeshIO.GLTF
{
	/// <summary>
	/// A typed view into a bufferView. A bufferView contains raw binary data.
	/// An accessor provides a typed view into a bufferView or a subset of a bufferView 
	/// similar to how WebGL's vertexAttribPointer() defines an attribute in a buffer.
	/// </summary>
	internal class GltfAccessor : GltfNamedAsset
	{
		/// <summary>
		/// The index of the bufferView. 
		/// When not defined, accessor must be initialized with zeros; 
		/// sparse property or extensions could override zeros with actual values.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public int bufferView { get; set; } = 0;
		/// <summary>
		/// The offset relative to the start of the bufferView in bytes.
		/// This must be a multiple of the size of the component datatype.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public int byteOffset { get; set; } = 0;
		/// <summary>
		/// The datatype of components in the attribute.
		/// All valid values correspond to WebGL enums. 
		/// The corresponding typed arrays are Int8Array, Uint8Array, Int16Array, Uint16Array, Uint32Array, and Float32Array, respectively.
		/// 5125 (UNSIGNED_INT) is only allowed when the accessor contains indices,
		/// i.e., the accessor is only referenced by primitive.indices.
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		public GltfComponentType componentType { get; set; }
		/// <summary>
		/// The offset relative to the start of the bufferView in bytes.
		/// This must be a multiple of the size of the component datatype.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public bool? normalized { get; set; } = false;
		/// <summary>
		/// The number of attributes (elements in the buffer) referenced by this accessor, 
		/// not to be confused with the number of bytes or number of components.
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		public int count { get; set; }
		/// <summary>
		/// Specifies if the attribute is a scalar, vector, or matrix.
		/// </summary>
		/// <remarks>
		/// Required: YES
		/// </remarks>
		/// <value> 
		/// "SCALAR", "VEC2", "VEC3", "VEC4", "MAT2", "MAT3", "MAT4"
		/// </value>
		public string type { get; set; }
		/// <summary>
		/// Maximum value of each component in this attribute. Array elements must be treated as having the same data type as accessor's componentType. Both min and max arrays have the same length. The length is determined by the value of the type property; it can be 1, 2, 3, 4, 9, or 16.
		/// normalized property has no effect on array values: they always correspond to the actual values stored in the buffer.When accessor is sparse, this property must contain max values of accessor data with sparse substitution applied.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		/// <value>
		/// number [1-16]
		/// </value>
		public List<double> max { get; set; }
		/// <summary>
		/// Minimum value of each component in this attribute. Array elements must be treated as having the same data type as accessor's componentType. Both min and max arrays have the same length. The length is determined by the value of the type property; it can be 1, 2, 3, 4, 9, or 16.
		/// normalized property has no effect on array values: they always correspond to the actual values stored in the buffer.When accessor is sparse, this property must contain min values of accessor data with sparse substitution applied.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		/// <value>
		/// number [1-16]
		/// </value>
		public List<double> min { get; set; }
		/// <summary>
		/// Sparse storage of attributes that deviate from their initialization value.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public GltfSparse sparse { get; set; }
	}
}
