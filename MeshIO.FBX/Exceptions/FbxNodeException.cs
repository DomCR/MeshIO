using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO.FBX.Exceptions
{
	[Serializable]
	public class FbxNodeException : Exception
	{
		public FbxNodeException() { }
		public FbxNodeException(string message) : base(message) { }
		public FbxNodeException(string message, Exception inner) : base(message, inner) { }
		protected FbxNodeException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
