using System;

namespace MeshIO
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
		Locked = 1,

		/// <summary>
		/// This is a user defined property
		/// </summary>
		UserDefined = 2,

		/// <summary>
		/// The property is animatable
		/// </summary>
		Animatable = 4,

		/// <summary>
		/// The property is animated
		/// </summary>
		Animated = 8,

		/// <summary>
		/// The property is marked as hidden
		/// </summary>
		Hidden = 16,
	}
}
