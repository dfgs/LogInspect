using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspectLib.Parsers
{
	public class LogParser
	{
		private Rule rule;
		private Regex regex;
		private Column[] columns;
		private InlineParser[] inlineParsers;

		public LogParser( Rule Rule,IEnumerable<Column> Columns)
		{
			this.rule = Rule;
			regex = new Regex(Rule.GetPattern());
			columns = Columns.ToArray();
			inlineParsers = columns.Select((item) => new InlineParser(item)).ToArray();
		}

		public Event? Parse(Log Log)
		{
			Match match;
			Event ev;
			List<Property> properties;
			Property property;
			Column column;

			match = regex.Match(Log.ToSingleLine());
			if (!match.Success) return null;

			properties = new List<Property>();
			for(int t=0;t<columns.Length;t++)
			{
				column = columns[t];
				property=new Property() { Name = column.Name };
				property.Value = column.GetValue(match.Groups[column.Name].Value);
				property.Inlines = inlineParsers[t].Parse(property.Value?.ToString()).ToArray();

				properties.Add(property);
			}

			ev = new Event(Log,rule, properties.ToArray() );

			return ev;
		}



	}
}
