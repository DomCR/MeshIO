using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.FBX.Nodes.Properties
{
	[Flags]
	public enum FbxPropertyFlag
	{
		UserDefined = 'U',
		Animatable = 'A',
		Animated = '+',
		Hidden = 'H',
		Locked = 'L',

	}
}
