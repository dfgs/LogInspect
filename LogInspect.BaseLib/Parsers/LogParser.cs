using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspect.BaseLib.Parsers
{
	public class LogParser:ILogParser
	{
		private List<Regex> items;
		private IEnumerable<Column> columns;

		public LogParser( IEnumerable<Column> Columns)
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
			Regex regex;

			regex = new Regex(Pattern, RegexOptions.Compiled);
			items.Add(regex);
		}
		
		/*protected object OnConvertValue(Column Column,string Value)
		{
			DateTime result;

			switch(Column.Type)
			{
				case ColumnType.String:return Value;
				case ColumnType.DateTime:
					if (!DateTime.TryParseExact(Value, Column.Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result)) return Value;
					return result;
				default:
					throw new InvalidCastException($"Invalid column type {Column.Type} for column {Column.Name}");
			}
		}*/

		public Event Parse(Log Log)
		{
			Match match;
			Event ev;
			string logLine;

			if (Log == null) throw new ArgumentNullException("Cannot parse null log");

			logLine = Log.ToSingleLine();

			foreach (Regex Regex in items)
			{
				match = Regex.Match(logLine);
				if (!match.Success) continue;
		
				ev = new Event();
				ev.LineIndex = Log.LineIndex;
				foreach (Column column in columns)
				{
					ev[column.Name] =  match.Groups[column.Name].Value.Trim();
				}
				return ev;
			}

			//this.Log(LogLevels.Warning, $"No pattern matched log: {logLine}");
			return null;
		}



	}
}
