using System;
using System.Collections.Generic;

namespace MeshIO.Formats.Gltf.Schema.V1;

internal class GltfV1Root
{
	public Dictionary<string, GltfAccessor> Accessors { get; } = new();

	public Dictionary<string, object> Animations { get; } = new();

	public object Asset { get; private set; }

	public Dictionary<string, object> Buffers { get; } = new();

	public Dictionary<string, object> BufferViews { get; } = new();

	public Dictionary<string, object> Cameras { get; } = new();

	public string[] ExtensionsUsed { get; private set; }

	public Dictionary<string, object> Images { get; } = new();

	public Dictionary<string, object> Materials { get; } = new();

	public Dictionary<string, object> Meshes { get; } = new();

	public Dictionary<string, object> Nodes { get; } = new();

	public Dictionary<string, object> Programs { get; } = new();

	public Dictionary<string, object> Samplers { get; } = new();

	public string Scene { get; private set; }

	public Dictionary<string, object> Scenes { get; } = new();

	public Dictionary<string, object> Shaders { get; } = new();

	public Dictionary<string, object> Skins { get; } = new();

	public Dictionary<string, object> Techniques { get; } = new();

	public Dictionary<string, object> Textures { get; } = new();

	public GltfV1Root(Dictionary<string, object> map)
	{
		var local = new Dictionary<string, object>(map, StringComparer.InvariantCultureIgnoreCase);
		foreach (var item in local)
		{
			var name = item.Key.ToLower();
			switch (name)
			{
				case "accessors":
					setMap(item.Value, this.Accessors);
					break;
				case "animations":
					setMap(item.Value, this.Animations);
					break;
				case "asset":
					Asset = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(item.Value.ToString());
					break;
				case "buffers":
					setMap(item.Value, this.Buffers);
					break;
				case "bufferviews":
					setMap(item.Value, this.BufferViews);
					break;
				case "cameras":
					setMap(item.Value, this.Cameras);
					break;
				case "textures":
					setMap(item.Value, this.Textures);
					break;
				case "extensionsused":
					ExtensionsUsed = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(item.Value.ToString());
					break;
				case "images":
					setMap(item.Value, this.Images);
					break;
				case "materials":
					setMap(item.Value, this.Materials);
					break;
				case "meshes":
					setMap(item.Value, this.Meshes);
					break;
				case "nodes":
					setMap(item.Value, this.Nodes);
					break;
				case "programs":
					setMap(item.Value, this.Programs);
					break;
				case "samplers":
					setMap(item.Value, this.Samplers);
					break;
				case "scene":
					Scene = item.Value.ToString();
					break;
				case "scenes":
					setMap(item.Value, this.Scenes);
					break;
				case "shaders":
					setMap(item.Value, this.Shaders);
					break;
				case "skins":
					setMap(item.Value, this.Skins);
					break;
				case "techniques":
					setMap(item.Value, this.Techniques);
					break;
				default:
					throw new NotImplementedException(name);
			}
		}
	}

	private Dictionary<string, object> getMap(string json)
	{
		return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
	}

	private void setMap<T>(object obj, Dictionary<string, T> arr)
	{
		var map = this.getMap(obj.ToString());
		foreach (var item in map)
		{
			var name = item.Key.ToLower();
			var json = item.Value.ToString();
			T t = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(item.Value.ToString());
			arr.Add(name, t);
		}
	}
}