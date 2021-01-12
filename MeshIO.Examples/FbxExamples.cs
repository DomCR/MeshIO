using MeshIO.FBX;
using MeshIO.FBX.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MeshIO.Examples
{
	public static class FbxExamples
	{
		public static void AsciiToBinary()
		{
			string pathI = Utils.GetFileDirectoryPath() + "\\FbxSamples\\objects_ascii_2014-2015.fbx";
			string pathOBin = Utils.GetFileDirectoryPath() + "\\FbxSamples\\objects_output_bin.fbx";
			string pathO = Utils.GetFileDirectoryPath() + "\\FbxSamples\\objects_output.fbx";

			FbxRoot root = FbxIO.ReadAsciiAsRootNode(pathI);
			FbxRootDocument rootDoc = new FbxRootDocument(root, true);

			//FbxIO.WriteBinary(rootDoc, pathOBin);
			//FbxIO.WriteAscii(rootDoc, pathO);
		}
	}
}
