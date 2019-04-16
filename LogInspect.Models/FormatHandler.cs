using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace LogInspect.Models
{
	[Serializable]
    public class FormatHandler
    {
		[XmlAttribute]
		public string Name
		{
			get;
			set;
		}

		[XmlAttribute]
		public string NameSpace
		{
			get;
			set;
		}

		[XmlAttribute]
		public string FileNamePattern
		{
			get;
			set;
		}

		/*[XmlArray]
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
		}*/

		[XmlArray]
		public List<string> DiscardLinePatterns
		{
			get;
			set;
		}
		[XmlArray]
		public List<string> LogPrefixPatterns
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
			//AppendLineToNextPatterns = new List<string>();
			//AppendLineToPreviousPatterns = new List<string>();
			LogPrefixPatterns = new List<string>();
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





		/*public ILogParser CreateLogParser(IRegexBuilder RegexBuilder)
		{
			ILogParser logParser;

			logParser = new LogParser(Columns.Select(item=>item.Name));
			foreach (Rule rule in Rules)
			{
				logParser.Add(RegexBuilder.Build(NameSpace, rule.GetPattern(),false),rule.Discard );
			}

			return logParser;
		}*/

		/*public IStringMatcher CreateDiscardLinesMatcher(IRegexBuilder RegexBuilder)
		{
			return CreateStringMatcher(RegexBuilder, DiscardLinePatterns);
		}
		public IStringMatcher CreateAppendLineToPreviousMatcher(IRegexBuilder RegexBuilder)
		{
			return CreateStringMatcher(RegexBuilder, AppendLineToPreviousPatterns);
		}
		public IStringMatcher CreateAppendLineToNextMatcher(IRegexBuilder RegexBuilder)
		{
			return CreateStringMatcher(RegexBuilder, AppendLineToNextPatterns);
		}*/


	}
}
