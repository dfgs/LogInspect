﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LogInspectLib
{
	[Serializable]
	public class InlineColoringRule
	{
		[XmlAttribute]
		public string Pattern
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
		public bool Underline
		{
			get;
			set;
		}
	}
}
