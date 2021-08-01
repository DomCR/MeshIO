using System;

namespace MeshIO.FBX
{
	[Flags]
	public enum PropertyFlags : byte
	{
		/// <summary>
		/// No flags
		/// </summary>
		None = 0,
		/// <summary>
		/// Not serializable
		/// </summary>
		/// <remarks>
		/// Fbx value : 'L'
		/// </remarks>
		Locked = 1,
		/// <summary>
		/// This is a user defined property
		/// </summary>
		/// <remarks>
		/// Fbx value : 'U'
		/// </remarks>
		UserDefined = 2,
		/// <summary>
		/// The property is animatable
		/// </summary>
		/// <remarks>
		/// Fbx value : 'A'
		/// </remarks>
		Animatable = 4,
		/// <summary>
		/// The property is animated
		/// </summary>
		/// <remarks>
		/// Fbx value : '+'
		/// </remarks>
		Animated = 8,
		/// <summary>
		/// The property is marked as hidden
		/// </summary>
		/// <remarks>
		/// Fbx value : 'H'
		/// </remarks>
		Hidden = 16, 
	}
}
