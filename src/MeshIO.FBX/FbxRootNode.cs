using MeshIO.FBX.Converters;
using System;

namespace MeshIO.FBX
{
	/// <summary>
	/// A top-level FBX node
	/// </summary>
	[Obsolete("This class will be removed")]
	public class FbxRootNode : FbxNodeCollection
	{
		/// <summary>
		/// Describes the format and data of the document
		/// </summary>
		/// <remarks>
		/// It isn't recommended that you change this value directly, because
		/// it won't change any of the document's data which can be version-specific.
		/// Most FBX importers can cope with any version.
		/// </remarks>
		public FbxVersion Version { get; set; } = FbxVersion.v7400;

		/// <summary>
		/// Create a <see cref="FbxRootNode"/> from an <see cref="Scene"/>.
		/// </summary>
		/// <param name="scene">Scene from where the <see cref="FbxRootNode"/> will be created.</param>
		/// <param name="version"><see cref="FbxVersion"/> format for the <see cref="FbxRootNode"/></param>
		public static FbxRootNode CreateFromScene(Scene scene, FbxVersion version = FbxVersion.v7400)
		{
			ISceneConverter converter = SceneConverterBase.GetConverter(scene, version);
			return converter.ToRootNode();
		}
	}
}
