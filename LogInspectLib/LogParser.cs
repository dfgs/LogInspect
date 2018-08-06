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
		private IEnumerable<SeverityMapping> severityMapping;

		public LogParser(Rule Rule,IEnumerable<SeverityMapping> SeverityMapping)
		{
			this.rule = Rule;
			regex = new Regex(Rule.GetPattern());
			columns = Rule.Tokens.Where(item=>item.Name!=null).Select(item=>item.Name).ToArray();
			this.severityMapping = SeverityMapping;
		}

		public Event? Parse(Log Log)
		{
			Match match;
			Event ev;
			Severity severity;
			Property[] properties;

			match = regex.Match(Log.ToSingleLine());
			if (!match.Success) return null;

			properties = columns.Select(item => new Property() { Name = item, Value = match.Groups[item].Value }).ToArray();

			severity = Severity.Debug;
			foreach(SeverityMapping mapping in severityMapping)
			{
				if (Regex.Match(match.Groups[mapping.Token].Value,mapping.Pattern).Success)
				{
					Enum.TryParse(mapping.Severity, out severity);
					break;
				}
			}
			ev = new Event(Log,rule,severity, properties );

			return ev;
		}



	}
}
