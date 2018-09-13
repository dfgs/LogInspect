using LogInspectLib.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace LogInspectLib
{
	[Serializable]
    public class FormatHandler
    {
		[XmlElement]
		public string Name
		{
			get;
			set;
		}
		[XmlElement]
		public string FileNamePattern
		{
			get;
			set;
		}

		[XmlArray]
		public List<string> AppendLineToPreviousPatterns
		{
			get;
			set;
		}


		[XmlArray]
		public List<string> AppendLineToNextPatterns
		{
			get;
			set;
		}

		[XmlArray]
		public List<string> DiscardLinePatterns
		{
			get;
			set;
		}

		[XmlArray]
		public List<Column> Columns
		{
			get;
			set;
		}
		[XmlAttribute]
		public string SeverityColumn
		{
			get;
			set;
		}

		[XmlAttribute]
		public string TimeStampColumn
		{
			get;
			set;
		}

		[XmlAttribute]
		public string DefaultColumn
		{
			get;
			set;
		}

		[XmlArray]
		public List<Rule> Rules
		{
			get;
			set;
		}


		[XmlArray]
		public List<EventColoringRule> EventColoringRules
		{
			get;
			set;
		}

		

		public FormatHandler()
		{
			AppendLineToNextPatterns = new List<string>();
			AppendLineToPreviousPatterns = new List<string>();
			DiscardLinePatterns = new List<string>();
			Rules = new List<Rule>();
			Columns = new List<Column>();
			EventColoringRules = new List<EventColoringRule>();
		}

		public void SaveToFile(string FileName)
		{
			using (FileStream stream = new FileStream(FileName, FileMode.Create))
			{
				SaveToStream(stream);
			}
		}
		public void SaveToStream(Stream Stream)
		{
			XmlSerializer serializer;

			serializer = new XmlSerializer(typeof(FormatHandler));
			serializer.Serialize(Stream, this);
		}

		public static FormatHandler LoadFromFile(string FileName)
		{
			using (FileStream stream = new FileStream(FileName, FileMode.Open))
			{
				return LoadFromStream(stream);
			}
		}
		public static FormatHandler LoadFromStream(Stream Stream)
		{
			XmlSerializer serializer;

			serializer = new XmlSerializer(typeof(FormatHandler));
			return (FormatHandler)serializer.Deserialize(Stream);
		}

		


		public IEnumerable<ILogParser> CreateLogParsers(IRegexBuilder RegexBuilder)
		{
			foreach (Rule rule in Rules)
			{
				yield return new LogParser(rule, Columns,RegexBuilder);
			}
		}



	}
}
