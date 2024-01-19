using MeshIO.Entities;

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
}
