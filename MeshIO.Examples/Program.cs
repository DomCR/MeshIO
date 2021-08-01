using MeshIO.Elements;
using MeshIO.FBX;
using MeshIO.FBX.Readers;
using System;
using System.IO;

namespace MeshIO.Examples
{
	class Program
	{
		static void Main(string[] args)
		{
			string pathI = @".\..\..\..\..\file_samples\fbx\objects_ascii_2014-2015.fbx";
			string pathO = @".\..\..\..\..\file_samples\fbx\objects_ascii_2014-2015_out.fbx";

			using (FbxReader reader = new FbxReader(pathI, ErrorLevel.Checked))
			{
				var root = reader.Parse();
			}

			//Scene scene = FbxIO.Read(pathI);
			//FbxIO.WriteAscii(scene, pathO);

			Console.WriteLine("Program finished");
		}
	}
}
