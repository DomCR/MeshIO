using System;

namespace MeshIO.FBX
{
	public interface IFbxWriter: IDisposable
	{
		/// <summary>
		/// Write an <see cref="FbxRootNode"/> into an fbx binary file.
		/// </summary>
		void WriteBinary(FbxRootNode root);

		/// <summary>
		/// Write an <see cref="Scene"/> into an fbx binary file.
		/// </summary>
		void WriteBinary(Scene scene);

		/// <summary>
		/// Write a <see cref="FbxRootNode"/> into a fbx an ascii file.
		/// </summary>
		void WriteAscii(FbxRootNode root);

		/// <summary>
		/// Write a <see cref="Scene"/> into a fbx an ascii file.
		/// </summary>
		void WriteAscii(Scene scene);
	}
}
