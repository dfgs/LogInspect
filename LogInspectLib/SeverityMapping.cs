using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LogInspectLib
{
	[Serializable]
	public class SeverityMapping
	{
		
		[XmlAttribute]
		public string Pattern
		{
			get;
			set;
		}

		[XmlAttribute]
		public string Severity
		{
			get;
			set;
		}

	}
}
