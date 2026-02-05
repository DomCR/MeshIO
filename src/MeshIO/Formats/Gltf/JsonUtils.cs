#if NET
using System.Text.Json;
using System.Text.Json.Serialization;
#endif

namespace MeshIO.Formats.Gltf;

internal static class JsonUtils
{
	public static T Deserialize<T>(string json)
	{
#if NET
		JsonSerializerOptions options = new JsonSerializerOptions();
		options.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;



		return System.Text.Json.JsonSerializer.Deserialize<T>(json);
#else
		return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
#endif
	}
}
