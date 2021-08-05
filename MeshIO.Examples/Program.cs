using MeshIO.Elements;
using MeshIO.FBX;
using MeshIO.GLTF;
using System;
using System.IO;

namespace MeshIO.Examples
{
	class Program
	{
		static void Main(string[] args)
		{
			//FbxExample();
			GltfExample();

			Console.WriteLine("Program finished");
		}

		static void FbxExample()
		{
			string pathI = @".\..\..\..\..\file_samples\fbx\objects_ascii_2014-2015.fbx";
			string pathO = @".\..\..\..\..\file_samples\fbx\objects_ascii_2014-2015_out.fbx";

			Scene Scene = FbxReader.Read(pathI, ErrorLevel.Checked);
			FbxWriter.WriteAscii(pathO, Scene);
		}

		static void GltfExample()
		{
			string pathI = @".\..\..\..\..\file_samples\gltf\2CylinderEngine.glb";
			string pathO = @".\..\..\..\..\file_samples\gltf\2CylinderEngine_out.glb";

			using (GltfReader reader = new GltfReader(pathI))
			{
				reader.Read();
			}
		}
	}
}
