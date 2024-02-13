namespace MeshIO.OBJ
{
	internal class ObjFileParser
	{
		public static bool ParseToken(string text, out ObjFileToken token)
		{
			token = ObjFileToken.Undefined;

			switch (text.ToLower())
			{
				case "o":
					token = ObjFileToken.Object;
					return true;
				case "v":
					token = ObjFileToken.Vertice;
					return true;
				case "f":
					token = ObjFileToken.Face;
					return true;
				case "vn":
					token = ObjFileToken.Normal;
					return true;
				case "vt":
					token = ObjFileToken.TextureVertice;
					return true;
				case "#":
					token = ObjFileToken.Comment;
					return true;
				default:
					return false;
			}
		}
	}
}
