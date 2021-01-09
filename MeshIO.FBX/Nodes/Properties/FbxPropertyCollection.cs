using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeshIO.FBX.Nodes.Properties
{
	//Properties70:  { }	
	//String format: Properties{0} 
	//{0} = version
	public class FbxPropertyCollection : FbxNodeReference, IEnumerable<FbxProperty>
	{
		public override string ClassName { get { return $"Properties{Version}"; } }
		public FbxVersion Version { get; }

		public int Count => m_properties.Count;
		public FbxProperty this[int index] { get => m_properties[index]; }
		public FbxProperty this[string name] { get => m_properties.FirstOrDefault(o => o.Name == name); }
		private List<FbxProperty> m_properties = new List<FbxProperty>();
		public FbxPropertyCollection()
		{

		}
		public FbxPropertyCollection(FbxNode node)
		{

		}
		public void Add(FbxProperty item)
		{
			if (m_properties.Contains(item))
				throw new ArgumentException($"Property: {item.Name} is already in the collection.");

			m_properties.Add(item);
		}

		public void Clear()
		{
			m_properties.Clear();
		}

		public bool Contains(FbxProperty item)
		{
			return m_properties.Contains(item);
		}

		public bool Remove(FbxProperty item)
		{
			return m_properties.Remove(item);
		}

		public IEnumerator<FbxProperty> GetEnumerator()
		{
			return m_properties.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_properties.GetEnumerator();
		}
	}
}
