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

		private IEnumerable<Column> columns;
		private IRegexBuilder regexBuilder;

		public LogParser(  IRegexBuilder RegexBuilder, IEnumerable<Column> Columns)
		{
			this.regexBuilder = RegexBuilder;
			this.items = new List<Regex>();
			this.columns = Columns;
		}

		public void Add(string DefaultNameSpace,string Pattern)
		{
			Regex regex;
			regex = regexBuilder.Build(DefaultNameSpace, Pattern);
			items.Add(regex);
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
				foreach(Column column in columns)
				{
					ev[column.Name] = column.ConvertValue(match.Groups[column.Name].Value);
				}

				return ev;

			}

			return null;
		}



	}
}
