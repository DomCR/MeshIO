namespace MeshIO.FBX.Converters.Mappers
{
	public interface IFbxMapper
	{
		string SectionName { get; }

		/// <summary>
		/// Map a <see cref="FbxNode"/> into a mapper
		/// </summary>
		/// <param name="node"></param>
		void Map(FbxNode node);
	}
}
