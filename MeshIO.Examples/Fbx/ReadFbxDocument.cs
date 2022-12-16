using MeshIO.Elements;
using MeshIO.Elements.Geometries;
using MeshIO.FBX;
using System.Collections.Generic;

namespace MeshIO.Examples.Fbx
{
	public static class ReadFbxDocument
	{
		public const string SamplesFolderPath = @".\..\..\..\..\file_samples\fbx\";

		public static Scene ReadDocument(string path)
		{
			return FbxReader.Read(path, ErrorLevel.Checked);
		}

		public static IEnumerable<Geometry> GetAllGeometryInTheScene(string path)
		{
			Scene scene = FbxReader.Read(path, ErrorLevel.Checked);

			//Iterate throgh all the nodes in the scene
			foreach (Node item in scene.Nodes)
			{
				//Here you can apply filter if you know the name of the node

				//Each node can contain geometric elements so you have to look in the elements contained in the main node
				foreach (Element c in item.Children)
				{
					//Check if the element is a geometric type
					if (!(c is Geometry geometry))
						continue;

					yield return geometry;
				}
			}
		}
	}
}
