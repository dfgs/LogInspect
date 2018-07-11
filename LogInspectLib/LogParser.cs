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
			columns = Rule.GetColumns().ToArray();
		}

		public Event Parse(Log Log)
		{
			Property property;
			Match match;
			Event ev;

			match = regex.Match(Log.ToSingleLine());
			if (!match.Success) return null;

			ev = new Event();
			ev.Log = Log;
			ev.Rule = rule;

			foreach(string column in columns)
			{
				property = new Property();
				property.Name = column;
				property.Value = match.Groups[column].Value;
				ev.Properties.Add(property);
			}

			return ev;
		}



	}
}
