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
		private List<(Regex Regex,bool Discard)> items;
		private IEnumerable<Column> columns;

		public LogParser( IEnumerable<Column> Columns)
		{
			if (Columns == null) throw new ArgumentNullException("Columns");
			this.items = new List<(Regex Regex, bool Discard)>();
			this.columns = Columns; 
		}

		public void Add(Regex Regex,bool Discard)
		{
			items.Add((Regex,Discard));
		}
		public void Add(string Pattern,bool Discard)
		{
			Regex regex;

			regex = new Regex(Pattern, RegexOptions.Compiled);
			items.Add((regex,Discard));
		}
		
		protected object OnConvertValue(Column Column,string Value)
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
		}

		public Event Parse(Log Log)
		{
			Match match;
			Event ev;
			string logLine;

			if (Log == null) throw new ArgumentNullException("Cannot parse null log");

			logLine = Log.ToSingleLine();

			foreach ((Regex Regex, bool Discard) tuple in items)
			{
				match = tuple.Regex.Match(logLine);
				if (!match.Success) continue;
				if (tuple.Discard) return null; // discard

				ev = new Event();
				ev.LineIndex = Log.LineIndex;
				foreach (Column column in columns)
				{
					ev[column.Name] = OnConvertValue(column, match.Groups[column.Name].Value.Trim());
				}
				return ev;
			}

			//this.Log(LogLevels.Warning, $"No pattern matched log: {logLine}");
			return null;
		}



	}
}
