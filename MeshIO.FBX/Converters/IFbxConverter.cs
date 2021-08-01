using MeshIO.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.FBX.Converters
{
	public interface IFbxConverter
	{
		FbxVersion Version { get; }

		FbxRootNode ToRootNode();
	}
}
