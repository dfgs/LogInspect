using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LogInspectLib
{
    public class LogReader : StreamReader
    {
		//private static string alwaysFail = "(?= a)[^ a]";
		private FormatHandler formatHandler;
		public FormatHandler FormatHandler
		{
			get { return formatHandler; }
		}

		private string line;

		private List<Regex> appendToNextRegexes;
		private List<Regex> appendToPreviousRegexes;
		private List<LogParser> logParsers;

		public LogReader(Stream Stream,FormatHandler FormatHandler):base(Stream)
        {
			this.formatHandler = FormatHandler;

			this.appendToNextRegexes = new List<Regex>();
			this.appendToPreviousRegexes = new List<Regex>();
			this.logParsers = new List<LogParser>();

			foreach (string pattern in FormatHandler.AppendToPreviousPatterns)
			{
				this.appendToPreviousRegexes.Add(new Regex(pattern));
			}
			foreach (string pattern in FormatHandler.AppendToNextPatterns)
			{
				this.appendToNextRegexes.Add(new Regex(pattern));
			}
			foreach(Rule rule in FormatHandler.Rules)
			{
				this.logParsers.Add(new LogParser(rule));
			}
		}


		private bool MustAppendToNextLine(string Line)
		{
			if (Line == null) return false;
			foreach (Regex regex in appendToNextRegexes)
			{
				if (regex.Match(Line).Success) return true;
			}
			return false; 
		}
		private bool MustAppendToPreviousLine(string Line)
		{
			if (Line == null) return false;
			foreach (Regex regex in appendToPreviousRegexes)
			{
				if (regex.Match(Line).Success) return true;
			}
			return false;
		}

		public Log ReadLog()
        {
  			List<string> lines;
 			bool mustAppend;

			lines = new List<string>();

			if (line == null) line = ReadLine();

			while(line!=null)
			{
				mustAppend = MustAppendToNextLine(line);
				lines.Add(line);
				line = ReadLine();
				if (!mustAppend) break;
			}
			while (line != null)
			{
				mustAppend = MustAppendToPreviousLine(line);
				if (!mustAppend) break;
				lines.Add(line);
				line = ReadLine();
			} 

			if (lines.Count == 0) return null;
			else return new Log(lines);
			
        }

		public Event ReadEvent()
		{
			Log log;
			Event ev;

			log = ReadLog();
			if (log == null) return null;

			foreach(LogParser parser in logParsers)
			{
				ev = parser.Parse(log);
				if (ev!=null) return ev;
			}

			ev = new Event();
			ev.Log = log;
			return ev;
		}



    }
}
