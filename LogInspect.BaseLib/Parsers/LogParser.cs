using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspect.BaseLib.Parsers
{
	public class LogParser:ILogParser
	{
		private List<(Regex Regex,bool Discard)> items;
		private IEnumerable<string> columns;

		public LogParser( IEnumerable<string> Columns)
		{
			if (Columns == null) throw new ArgumentNullException("Columns");
			this.items = new List<(Regex Regex, bool Discard)>();
			this.columns = Columns; 
		}

		public void Add(Regex Regex,bool Discard)
		{
			items.Add((Regex,Discard));
		}
		public void Add(string Pattern,bool Discard)
		{
			Regex regex;

			regex = new Regex(Pattern, RegexOptions.Compiled);
			items.Add((regex,Discard));
		}
		

		public Event Parse(Log Log)
		{
			Match match;
			Event ev;
			string logLine;

			if (Log == null) throw new ArgumentNullException("Cannot parse null log");

			logLine = Log.ToSingleLine();

			foreach ((Regex Regex, bool Discard) tuple in items)
			{
				match = tuple.Regex.Match(logLine);
				if (!match.Success) continue;
				if (tuple.Discard) return null; // discard

				ev = new Event();
				ev.LineIndex = Log.LineIndex;
				foreach (string column in columns)
				{
					ev[column] = match.Groups[column].Value.Trim();
				}
				return ev;
			}

			//this.Log(LogLevels.Warning, $"No pattern matched log: {logLine}");
			return null;
		}



	}
}
