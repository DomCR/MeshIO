using MeshIO.FBX.Exceptions;
using MeshIO.FBX.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;

namespace MeshIO.FBX
{
	/// <summary>
	/// A top-level FBX node.
	/// </summary>
	public class FbxRoot : FbxNodeCollection
	{
		/// <summary>
		/// Describes the format and data of the fbx document.
		/// </summary>
		/// <remarks>
		/// It isn't recommended that you change this value directly, because
		/// it won't change any of the document's data which can be version-specific.
		/// Most FBX importers can cope with any version.
		/// </remarks>
		public FbxVersion Version { get; set; } = FbxVersion.v7400;
		/// <summary>
		/// Gets a main node in the document.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public FbxNode this[FbxMainNodeType type]
		{
			get { return this[type.ToString()]; }
			set { this[type.ToString()] = value; }
		}
		/// <summary>
		/// Default constructor, it will not generate any subnodes.
		/// </summary>
		public FbxRoot() { }
		//**********************************************************************************
	}
}
