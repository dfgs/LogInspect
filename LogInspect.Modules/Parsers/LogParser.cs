using LogInspect.Models;
using LogLib;
using ModuleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspect.Modules.Parsers
{
	public class LogParser:Module,ILogParser
	{
		private List<(Regex Regex,bool Discard)> items;
		private IEnumerable<string> columns;

		public LogParser(ILogger Logger, IEnumerable<string> Columns):base(Logger)
		{
			AssertParameterNotNull(Columns,"Columns", out columns);
			this.items = new List<(Regex Regex, bool Discard)>();
		}

		public void Add(Regex Regex,bool Discard)
		{
			items.Add((Regex,Discard));
		}
		public void Add(string Pattern,bool Discard)
		{
			Regex regex;

			if (!Try(() => new Regex(Pattern, RegexOptions.Compiled)).OrWarn(out regex, $"Failed to compile pattern {Pattern}")) return;
			items.Add((regex,Discard));
		}
		

		public Event Parse(Log Log)
		{
			Match match;
			Event ev;
			string logLine;

			if (Log == null)
			{
				this.Log(LogLevels.Error, "Cannot parse null log");
				return null;
			}

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

			this.Log(LogLevels.Warning, $"No pattern matched log: {logLine}");
			return null;
		}



	}
}
