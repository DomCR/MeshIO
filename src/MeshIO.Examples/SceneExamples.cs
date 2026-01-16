using MeshIO.Entities.Geometries;
using System.Collections.Generic;

namespace MeshIO.Examples
{
	/// <summary>
	/// Scene manipulation examples
	/// </summary>
	public static class SceneExamples
	{
		/// <summary>
		/// Get all the geometric elements in a scene
		/// </summary>
		/// <param name="scene"></param>
		/// <returns></returns>
		public static IEnumerable<Geometry> GetAllGeometryInTheScene(Scene scene)
		{
			//Iterate throgh all the nodes in the scene
			foreach (Element3D item in scene.RootNode.Children)
			{
				//Check if the element is a geometric type
				if (item is Geometry geometry)
				{
					yield return geometry;
				}

				if (item is Node node)
				{
					//Each node can contain geometric elements so you have to look in the elements contained in the main node
					foreach (Element3D c in node.Children)
					{

					}
				}
			}
		}
	}
}
