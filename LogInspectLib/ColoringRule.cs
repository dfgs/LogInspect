using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LogInspectLib
{
	[Serializable]
	public class ColoringRule
	{
		[XmlAttribute]
		public string Column
		{
			get;
			set;
		}

		[XmlAttribute]
		public string Pattern
		{
			get;
			set;
		}

		[XmlAttribute]
		public string Background
		{
			get;
			set;
		}

	}
}
