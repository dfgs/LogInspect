using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspectLib
{
	public class LogParser
	{
		private Rule rule;
		private Regex regex;
		private Column[] columns;

		public LogParser( Rule Rule,IEnumerable<Column> Columns)
		{
			this.rule = Rule;
			regex = new Regex(Rule.GetPattern());
			columns = Columns.ToArray();
		}

		public Event? Parse(Log Log)
		{
			Match match;
			Event ev;
			Property[] properties;

			match = regex.Match(Log.ToSingleLine());
			if (!match.Success) return null;

			properties = columns.Select(item => new Property() { Name = item.Name, Value = item.GetValue( match.Groups[item.Name].Value) }).ToArray();

			ev = new Event(Log,rule, properties );

			return ev;
		}



	}
}
