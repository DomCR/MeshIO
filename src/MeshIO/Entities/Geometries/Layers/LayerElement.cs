﻿using System;
using System.Collections.Generic;

namespace MeshIO.Entities.Geometries.Layers
{
	public abstract class LayerElement
	{
		/// <summary>
		/// Name of the layer
		/// </summary>
		public string Name { get; set; } = string.Empty;

		public MappingMode MappingMode { get; set; }

		public ReferenceMode ReferenceMode { get; set; }

		public List<int> Indices { get; } = new List<int>();

		public Geometry Owner { get; set; }

		public LayerElement() : this(MappingMode.AllSame, ReferenceMode.IndexToDirect)
		{
		}

		public LayerElement(MappingMode mappingMode, ReferenceMode referenceMode)
		{
			this.MappingMode = mappingMode;
			this.ReferenceMode = referenceMode;
		}

		[Obsolete]
		public LayerElement(Geometry owner) : this()
		{
			this.Owner = owner;
		}
	}
}
