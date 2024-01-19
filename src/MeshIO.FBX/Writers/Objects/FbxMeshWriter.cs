using CSMath;
using MeshIO.Entities;
using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using MeshIO.FBX.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO.FBX.Writers.Objects
{
	internal class FbxMeshWriter : FbxEntityWriter<Mesh>
	{
		public override string FbxTypeName { get; } = FbxFileToken.Mesh;

		public FbxMeshWriter(Mesh element) : base(element)
		{
		}

		protected override void writeObjectBody(FbxFileWriterBase builder, IFbxStreamWriter writer)
		{
			base.writeObjectBody(builder, writer);

			if (this._element.Vertices.Any())
			{
				double[] arr = _element.Vertices.SelectMany(x => x.ToEnumerable()).ToArray();
				writer.WritePairNodeValue("Vertices", arr);
				writer.WritePairNodeValue("PolygonVertexIndex", polygonsArray(this._element));

				if (this._element.Edges.Any())
				{
					writer.WritePairNodeValue("Edges", arr);
				}
			}

			writer.WritePairNodeValue("GeometryVersion", 124);

			this.writeLayers(builder, writer);
		}

		private void writeLayers(FbxFileWriterBase builder, IFbxStreamWriter writer)
		{
			List<string> layers = new List<string>();

			foreach (LayerElement layer in this._element.Layers)
			{
				this.writeLayer(layer, builder, writer);

				layers.Add(layer.GetFbxName());
			}

			writer.WriteName("Layer");
			writer.WriteValue(0);
			writer.WriteOpenBracket();
			writer.WritePairNodeValue(FbxFileToken.Version, 100);


			foreach (string name in layers)
			{
				writer.WriteName("LayerElement");
				writer.WriteOpenBracket();

				writer.WritePairNodeValue("Type", name);
				writer.WritePairNodeValue("TypedIndex", 0);

				writer.WriteCloseBracket();
				writer.WriteEmptyLine();
			}

			writer.WriteCloseBracket();
			writer.WriteEmptyLine();
		}

		private void writeLayer<T>(T layer, FbxFileWriterBase builder, IFbxStreamWriter writer)
			where T : LayerElement
		{
			writer.WriteName(layer.GetFbxName());
			writer.WriteValue(0);
			writer.WriteOpenBracket();
			writer.WritePairNodeValue(FbxFileToken.Version, 100);
			writer.WritePairNodeValue("Name", layer.Name);
			writer.WritePairNodeValue("MappingInformationType", layer.MappingMode.GetFbxName());
			writer.WritePairNodeValue("ReferenceInformationType", layer.ReferenceMode.GetFbxName());

			string indexesName = string.Empty;
			switch (layer)
			{
				case LayerElementBinormal bnormals:
					writer.WritePairNodeValue("Binormals", bnormals.Normals.SelectMany(x => x.ToEnumerable()));
					writer.WritePairNodeValue("BinormalsW", bnormals.Weights);
					indexesName = "BinormalsIndex";
					break;
				case LayerElementNormal normals:
					writer.WritePairNodeValue("Normals", normals.Normals.SelectMany(x => x.ToEnumerable()));
					writer.WritePairNodeValue("NormalsW", normals.Weights);
					indexesName = "NormalsIndex";
					break;
				case LayerElementMaterial material:
					writer.WritePairNodeValue("Materials", material.Indexes);
					break;
				case LayerElementTangent tangents:
					writer.WritePairNodeValue("Tangents", tangents.Tangents.SelectMany(x => x.ToEnumerable()));
					writer.WritePairNodeValue("TangentsW", tangents.Weights);
					indexesName = "TangentsIndex";
					break;
				case LayerElementUV uv:
					writer.WritePairNodeValue("UV", uv.UV.SelectMany(x => x.ToEnumerable()));
					indexesName = "UVIndex";
					break;
				default:
					break;
			}

			if (layer.ReferenceMode != ReferenceMode.Direct && layer.Indexes.Any() && layer is not LayerElementMaterial)
			{
				writer.WritePairNodeValue(indexesName, layer.Indexes);
			}

			writer.WriteCloseBracket();
			writer.WriteEmptyLine();
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
