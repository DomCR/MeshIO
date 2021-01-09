using System;
using System.Collections.Generic;
using System.Text;

namespace MeshIO
{
	public class Material
	{
		public string Name { get; set; }
		public Color Color { get; set; } = new Color();
		/// <summary>
		/// Percentage of transparency of the material, 100% is invisible.
		/// </summary>
		/// <exception cref="ArgumentException"></exception>
		public float Transparency
		{
			get { return m_transparency; }
			set
			{
				if (value > 100)
					throw new ArgumentException("Value cannot be higher than 100.");
				if (value < 0)
					throw new ArgumentException("Value cannot be negative.");

				m_transparency = value;
			}
		}
		public float m_transparency;
		public Material()
		{
			Name = Guid.NewGuid().ToString("N").ToUpper();
		}
		public Material(string name) : this()
		{
			Name = name;
		}
		public Material(string name, int transparency) : this(name)
		{
			Transparency = transparency;
		}
		public Material(string name, Color color) : this(name, color, 0) { }
		public Material(string name, Color color, int transparency) : this(name, transparency)
		{
			Color = color;
		}
	}
}
