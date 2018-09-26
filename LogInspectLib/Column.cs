﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LogInspectLib
{
	[Serializable]
	public class Column
	{
		
		[XmlAttribute]
		public string Name
		{
			get;
			set;
		}

		[XmlAttribute]
		public string Type
		{
			get;
			set;
		}

		[XmlAttribute]
		public string Format
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

		[XmlAttribute]
		public bool IsFilterItemSource
		{
			get;
			set;
		}

		[XmlArray]
		public List<string> InlinePatternNames
		{
			get;
			set;
		}

		public Column()
		{
			InlinePatternNames = new List<string>();
		}

		

	}
}
