using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.GLTF
{
	/// <summary>
	/// Base class for Gltf assets.
	/// </summary>
	internal abstract class GltfBase 
	{
		/// <summary>
		/// Dictionary object with extension-specific objects.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public JObject extensions { get; set; }
		/// <summary>
		/// Application-specific data.
		/// </summary>
		/// <remarks>
		/// Required: NO
		/// </remarks>
		public JObject extras { get; set; }
	}
}
