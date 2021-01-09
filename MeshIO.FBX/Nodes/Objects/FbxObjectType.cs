using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.FBX.Nodes.Objects
{
	public enum FbxObjectType
	{
		Unknown = -1,
		Null,
		Marker,
		Skeleton,
		Mesh,
		Nurbs,
		Patch,
		Camera,
		CameraStereo,
		CameraSwitcher,
		Light,
		OpticalReference,
		OpticalMarker,
		NurbsCurve,
		TrimNurbsSurface,
		Boundary,
		NurbsSurface,
		Shape,
		LODGroup,
		SubDiv,
		CachedEffect,
		Line,

		ObjectMetaData	//Seen in some files
	}
}
