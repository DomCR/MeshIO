namespace MeshIO.Formats.Gltf;

internal static class JsonUtils
{
	public static T Deserialize<T>(string json)
	{
		return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
	}
}
