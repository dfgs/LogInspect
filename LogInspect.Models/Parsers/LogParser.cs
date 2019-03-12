using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspect.Models.Parsers
{
	public class LogParser:ILogParser
	{
		private List<Tuple<Regex,bool>> items;
		private IEnumerable<string> columns;

		public LogParser(IEnumerable<string> Columns)
		{
			if (Columns == null) throw new ArgumentNullException("Columns");
			this.items = new List<Tuple<Regex, bool>>();
			this.columns = Columns;
		}

		public void Add(Regex Regex,bool Discard)
		{
			items.Add(new Tuple<Regex, bool>(Regex,Discard));
		}
		public void Add(string Pattern,bool Discard)
		{
			items.Add(new Tuple<Regex, bool>(new Regex(Pattern, RegexOptions.Compiled),Discard));
		}
		

		public Event Parse(Log Log)
		{
			Match match;
			Event ev;
			string logLine;

			ev = new Event();
			ev.LineIndex = Log.LineIndex;
			logLine = Log.ToSingleLine();

			foreach (Tuple<Regex,bool> tuple in items)
			{
				match = tuple.Item1.Match(logLine);
				if (!match.Success) continue;
				if (tuple.Item2) return null;	// discard

				foreach(string column in columns)
				{
					ev[column] = match.Groups[column].Value.Trim();
				}

				return ev;
			}

			return ev;
		}



	}
}
