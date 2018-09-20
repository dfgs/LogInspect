using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspectLib.Parsers
{
	public class LogParser:ILogParser
	{
		private List<Regex> items;
		private IEnumerable<string> columns;

		public LogParser(IEnumerable<string> Columns)
		{
			if (Columns == null) throw new ArgumentNullException("Columns");
			this.items = new List<Regex>();
			this.columns = Columns;
		}

		public void Add(Regex Regex)
		{
			items.Add(Regex);
		}
		public void Add(string Pattern)
		{
			items.Add(new Regex(Pattern, RegexOptions.Compiled));
		}
		public void Add(IEnumerable<Regex> Regexes)
		{
			items.AddRange(Regexes);
		}

		public Event Parse(Log Log)
		{
			Match match;
			Event ev;

			
			foreach (Regex regex in items)
			{
				match = regex.Match(Log.ToSingleLine());
				if (!match.Success) continue;

				ev = new Event();
				ev.LineIndex = Log.LineIndex;
				foreach(string column in columns)
				{
					ev[column] = match.Groups[column].Value;
				}

				return ev;
			}

			return null;
		}



	}
}
