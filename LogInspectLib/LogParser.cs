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
		private string[] columns;

		public LogParser(Rule Rule)
		{
			this.rule = Rule;
			regex = new Regex(Rule.GetPattern());
			columns = Rule.Tokens.Where(item=>item.Name!=null).Select(item=>item.Name).ToArray();
		}

		public Event? Parse(Log Log)
		{
			Match match;
			Event ev;
			Property[] properties;

			match = regex.Match(Log.ToSingleLine());
			if (!match.Success) return null;

			properties = columns.Select(item => new Property() { Name = item, Value = match.Groups[item].Value }).ToArray();

			ev = new Event(Log,rule, properties );

			return ev;
		}



	}
}
