using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LogInspectLib
{
	[Serializable]
	public class Token
	{
		[XmlAttribute]
		public string Pattern
		{
			get;
			set;
		}
		[XmlAttribute]
		public string Name
		{
			get;
			set;
		}
		[XmlAttribute]
		public double Width
		{
			get;
			set;
		}
		[XmlAttribute]
		public string Alignment
		{
			get;
			set;
		}

		public string GetPattern()
		{
			if (Name==null) return $"({Pattern})";
			else return $"(?<{Name}>{Pattern})";
		}

	}
}
