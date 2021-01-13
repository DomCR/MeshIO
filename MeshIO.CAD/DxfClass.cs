using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.CAD
{
	[Flags]
	public enum ProxyFlags : ushort
	{
		/// <summary>
		/// None.
		/// </summary>
		None = 0,
		/// <summary>
		/// Erase allowed.
		/// </summary>
		EraseAllowed = 1,
		/// <summary>
		/// Transform allowed.
		/// </summary>
		TransformAllowed = 2,
		/// <summary>
		/// Clor change allowed.
		/// </summary>
		ColorChangeAllowed = 4,
		/// <summary>
		/// Layer change allowed.
		/// </summary>
		LayerChangeAllowed = 8,
		/// <summary>
		/// Line type change allowed.
		/// </summary>
		LinetypeChangeAllowed = 16, 
		/// <summary>
		/// Line type scale change allowed.
		/// </summary>
		LinetypeScaleChangeAllowed = 32, 
		/// <summary>
		/// Visibility change allowed.
		/// </summary>
		VisibilityChangeAllowed = 64,
		/// <summary>
		/// Cloning allowed.
		/// </summary>
		CloningAllowed = 128,
		/// <summary>
		/// Line weight change allowed.
		/// </summary>
		LineweightChangeAllowed = 256, 
		/// <summary>
		/// Plot Style Name change allowed.
		/// </summary>
		PlotStyleNameChangeAllowed = 512, 
		/// <summary>
		/// Disables proxy warning dialog
		/// </summary>
		DisablesProxyWarningDialog = 1024, 
		/// <summary>
		/// A R13 format proxy.
		/// </summary>
		R13FormatProxy = 32768, 
	}

	public class DxfClass
	{
		public short ClassNumber { get; internal set; }
		public ProxyFlags ProxyFlags { get; internal set; }
		public string ApplicationName { get; internal set; }
		public string CPlusPlusClassName { get; internal set; }
		public string DxfName { get; internal set; }
		public bool WasAZombie { get; internal set; }
		public short ItemClassId { get; internal set; }
		/// <inheritdoc/>
		public override string ToString()
		{
			return $"{DxfName}:{ClassNumber}";
		}
	}
}
