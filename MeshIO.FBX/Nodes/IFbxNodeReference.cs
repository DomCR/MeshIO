using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.FBX.Nodes
{
	public interface IFbxNodeReference
	{
		/// <summary>
		/// Converts this object into an fbx node structure.
		/// </summary>
		/// <remarks>
		/// Default name based on <see cref="FbxVersion.v7400"/>
		/// </remarks>
		/// <returns>null if the object is empty.</returns>
		FbxNode ToFbxNode();
		/// <summary>
		/// Converts this object into an fbx node structure.
		/// </summary>
		/// <param name="version">Node version.</param>
		/// <returns>null if the object is empty.</returns>
		FbxNode ToFbxNode(FbxVersion version);
		///// <summary>
		///// Modify the object based on an fbx node.
		///// </summary>
		///// <param name="node"></param>
		//void FromFbxNode(FbxNode node);
	}

	public interface IFbxNamedNode
	{
		/// <summary>
		/// Name of the node in a fbx document.
		/// </summary>
		/// <remarks>
		/// Default name based on <see cref="FbxVersion.v7400"/>
		/// </remarks>
		string ClassName { get; }
		/// <summary>
		/// Name of the node by version.
		/// </summary>
		/// <param name="version"></param>
		/// <returns></returns>
		string GetClassName(FbxVersion version);
	}
}
