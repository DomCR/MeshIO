using CSMath;
using MeshIO.Entities;
using MeshIO.Entities.Geometries;
using MeshIO.Entities.Geometries.Layers;
using MeshIO.FBX.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace MeshIO.FBX.Writers.Objects
{
	internal abstract class FbxEntityWriter<T> : FbxObjectWriterBase<T>
		where T : Entity
	{
		public override string FbxObjectName { get; } = FbxFileToken.Geometry;

		protected FbxEntityWriter(T element) : base(element)
		{
		}
	}

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

			this.writeMaterial(builder, writer);

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

		private void writeMaterial(FbxFileWriterBase builder, IFbxStreamWriter writer)
		{
			return;

			if (!this._element.ParentNodes.Any())
			{
				return;
			}
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

			switch (layer)
			{
				case LayerElementNormal normals:
					writer.WritePairNodeValue("Normals", normals.Normals.SelectMany(x => x.ToEnumerable()).ToArray());
					writer.WritePairNodeValue("NormalsW", normals.Weights.ToArray());
					break;
				case LayerElementUV uv:
					writer.WritePairNodeValue("UV", uv.UV.SelectMany(x => x.ToEnumerable()).ToArray());
					break;
				default:
					break;
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
