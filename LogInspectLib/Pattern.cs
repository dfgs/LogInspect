using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LogInspectLib
{
	[Serializable]
	public class Pattern	
	{
		[XmlAttribute]
		public string Name
		{
			get;
			set;
		}

		[XmlAttribute]
		public string Value
		{
			get;
			set;
		}

		[XmlAttribute]
		public string Foreground
		{
			get;
			set;
		}

		[XmlAttribute]
		public bool Italic
		{
			get;
			set;
		}

		[XmlAttribute]
		public bool Bold
		{
			get;
			set;
		}

		[XmlAttribute]
		public bool Underline
		{
			get;
			set;
		}

		public Pattern()
		{

		}
		
	}


}
