using CSMath;
using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using MeshIO.FBX.Extensions;
using MeshIO.FBX.Readers;
using MeshIO.FBX.Writers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO.FBX.Templates
{
	internal class FbxMeshTemplate : FbxGeometryTemplate<Mesh>
	{
		public override string FbxTypeName { get { return FbxFileToken.Mesh; } }

		public FbxMeshTemplate(Mesh mesh) : base(mesh) { }

		public FbxMeshTemplate(FbxNode node) : base(node, new Mesh())
		{
		}

		public override void Build(FbxFileBuilderBase builder)
		{
			base.Build(builder);

			readPolygons();
			readEdges();
		}

		protected override void addObjectBody(FbxNode node, FbxFileWriterBase writer)
		{
			node.Add("GeometryVersion", 124);

			base.addObjectBody(node, writer);

			if (this._element.Vertices.Any())
			{
				double[] arr = _element.Vertices.SelectMany(x => x.ToEnumerable()).ToArray();
				node.Add("Vertices", arr);
				node.Add("PolygonVertexIndex", polygonsArray(this._element));

				if (this._element.Edges.Any())
				{
					node.Add("Edges", this._element.Edges.ToArray());
				}
			}

			this.writeLayers(node);
		}

		private void writeLayers(FbxNode node)
		{
			List<string> layerNames = new List<string>();

			foreach (LayerElement layer in this._element.Layers)
			{
				node.Nodes.Add(this.writeLayer(layer));

				layerNames.Add(layer.GetFbxName());
			}

			FbxNode layers = node.Add("Layer", 0);
			layers.Add(FbxFileToken.Version, 100);

			foreach (string name in layerNames)
			{
				FbxNode layerElement = layers.Add("LayerElement");

				layerElement.Add("Type", name);
				layerElement.Add("TypedIndex", 0);
			}
		}

		private FbxNode writeLayer<T>(T layer)
			where T : LayerElement
		{
			FbxNode node = new FbxNode(layer.GetFbxName(), 0);
			node.Add(FbxFileToken.Version, 100);
			node.Add("Name", layer.Name);
			node.Add("MappingInformationType", layer.MappingMode.GetFbxName());
			node.Add("ReferenceInformationType", layer.ReferenceMode.GetFbxName());

			string indexesName = string.Empty;
			switch (layer)
			{
				case LayerElementBinormal bnormals:
					node.Add("Binormals", bnormals.Normals.SelectMany(x => x.ToEnumerable()).ToArray());
					node.Add("BinormalsW", bnormals.Weights.ToArray());
					indexesName = "BinormalsIndex";
					break;
				case LayerElementNormal normals:
					node.Add("Normals", normals.Normals.SelectMany(x => x.ToEnumerable()).ToArray());
					node.Add("NormalsW", normals.Weights.ToArray());
					indexesName = "NormalsIndex";
					break;
				case LayerElementMaterial material:
					node.Add("Materials", material.Indexes.ToArray());
					break;
				case LayerElementTangent tangents:
					node.Add("Tangents", tangents.Tangents.SelectMany(x => x.ToEnumerable()).ToArray());
					node.Add("TangentsW", tangents.Weights.ToArray());
					indexesName = "TangentsIndex";
					break;
				case LayerElementUV uv:
					node.Add("UV", uv.UV.SelectMany(x => x.ToEnumerable()).ToArray());
					indexesName = "UVIndex";
					break;
				default:
					break;
			}

			if (layer.ReferenceMode != ReferenceMode.Direct && layer.Indexes.Any() && layer is not LayerElementMaterial)
			{
				node.Add(indexesName, layer.Indexes.ToArray());
			}

			return node;
		}

		private void readEdges()
		{
			if (FbxNode.TryGetNode("Edges", out FbxNode edges))
			{
				_element.Edges.AddRange(toArr<int>(edges.Value as IEnumerable));
			}
		}

		private void readPolygons()
		{
			if (FbxNode.TryGetNode("PolygonVertexIndex", out FbxNode polygons))
			{
				_element.Polygons = mapPolygons(polygons.Value as int[]);
			}
		}

		protected List<Polygon> mapPolygons(int[] arr)
		{
			List<Polygon> polygons = new List<Polygon>();

			if (arr == null)
				return polygons;

			//Check if the arr are faces or quads
			if (arr[2] < 0)
			{
				for (int i = 2; i < arr.Length; i += 3)
				{
					Triangle tmp = new Triangle(
						arr[i - 2],
						arr[i - 1],
						//Substract a unit to the last
						Math.Abs(arr[i]) - 1);

					polygons.Add(tmp);
				}
			}
			//Quads
			else if (arr[3] < 0)
			{
				for (int i = 3; i < arr.Length; i += 4)
				{
					Quad tmp = new Quad(
						Math.Abs(arr[i - 3]),
						Math.Abs(arr[i - 2]),
						Math.Abs(arr[i - 1]),
						//Substract a unit to the last
						Math.Abs(arr[i]) - 1);

					polygons.Add(tmp);
				}
			}

			return polygons;
		}

		protected int[] polygonsArray(Mesh mesh)
		{
			List<int> arr = new List<int>();

			//Check if the polygons list is empty
			if (!mesh.Polygons.Any())
				return arr.ToArray();

			if (mesh.Polygons.First() is Triangle)
			{
				foreach (Triangle t in mesh.Polygons)
				{
					arr.Add((int)t.Index0);
					arr.Add((int)t.Index1);
					arr.Add(-((int)t.Index2 + 1));
				}
			}
			else
			{
				foreach (Quad t in mesh.Polygons)
				{
					arr.Add((int)t.Index0);
					arr.Add((int)t.Index1);
					arr.Add((int)t.Index2);
					arr.Add(-((int)t.Index3 + 1));
				}
			}

			return arr.ToArray();
		}
	}
}
