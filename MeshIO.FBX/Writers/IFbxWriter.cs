using MeshIO.Elements;
using MeshIO.FBX.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshIO.FBX
{
	public interface IFbxWriter
	{
		FbxRootNode GetRootNode();
		void WriteBinary();
		void WriteAscii();
	}
}
