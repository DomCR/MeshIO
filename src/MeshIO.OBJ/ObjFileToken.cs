﻿using CSMath;
using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO.OBJ
{
	internal enum ObjFileToken
	{
		Undefined = 0,
		/// <summary>
		/// o
		/// </summary>
		Object,
		/// <summary>
		/// v
		/// </summary>
		Vertice,
		/// <summary>
		/// vt
		/// </summary>
		TextureVertice,
		/// <summary>
		/// vn
		/// </summary>
		Normal,
		/// <summary>
		/// f
		/// </summary>
		Face,
	}
}
