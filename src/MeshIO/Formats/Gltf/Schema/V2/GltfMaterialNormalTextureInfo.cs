namespace MeshIO.Formats.Gltf.Schema.V2;

public class GltfMaterialNormalTextureInfo : IGltfTextureInfo
{
	/// <summary>
	/// Dictionary object with extension-specific objects.
	/// </summary>
	[Newtonsoft.Json.JsonPropertyAttribute("extensions")]
	public System.Collections.Generic.Dictionary<string, object> Extensions
	{
		get
		{
			return this._extensions;
		}
		set
		{
			this._extensions = value;
		}
	}

	/// <summary>
	/// Application-specific data.
	/// </summary>
	[Newtonsoft.Json.JsonPropertyAttribute("extras")]
	public GltfExtras Extras
	{
		get
		{
			return this._extras;
		}
		set
		{
			this._extras = value;
		}
	}

	/// <summary>
	/// The index of the texture.
	/// </summary>
	[Newtonsoft.Json.JsonRequiredAttribute()]
	[Newtonsoft.Json.JsonPropertyAttribute("index")]
	public string Index
	{
		get
		{
			return this._index;
		}
		set
		{
			this._index = value;
		}
	}

	/// <summary>
	/// The scalar multiplier applied to each normal vector of the normal texture.
	/// </summary>
	[Newtonsoft.Json.JsonPropertyAttribute("scale")]
	public float Scale
	{
		get
		{
			return this._scale;
		}
		set
		{
			this._scale = value;
		}
	}

	/// <summary>
	/// The set index of texture's TEXCOORD attribute used for texture coordinate mapping.
	/// </summary>
	[Newtonsoft.Json.JsonPropertyAttribute("texCoord")]
	public int TexCoord
	{
		get
		{
			return this._texCoord;
		}
		set
		{
			if ((value < 0))
			{
				throw new System.ArgumentOutOfRangeException("TexCoord", value, "Expected value to be greater than or equal to 0");
			}
			this._texCoord = value;
		}
	}

	/// <summary>
	/// Backing field for Extensions.
	/// </summary>
	private System.Collections.Generic.Dictionary<string, object> _extensions;

	/// <summary>
	/// Backing field for Extras.
	/// </summary>
	private GltfExtras _extras;

	/// <summary>
	/// Backing field for Index.
	/// </summary>
	private string _index;

	/// <summary>
	/// Backing field for Scale.
	/// </summary>
	private float _scale = 1F;

	/// <summary>
	/// Backing field for TexCoord.
	/// </summary>
	private int _texCoord = 0;

	public bool ShouldSerializeExtensions()
	{
		return ((_extensions == null)
					== false);
	}

	public bool ShouldSerializeExtras()
	{
		return ((_extras == null)
					== false);
	}

	public bool ShouldSerializeScale()
	{
		return ((_scale == 1F)
					== false);
	}

	public bool ShouldSerializeTexCoord()
	{
		return ((_texCoord == 0)
					== false);
	}
}