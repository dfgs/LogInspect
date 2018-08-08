using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLib
{
	public struct Event
	{
		public Log Log
		{
			get;
			private set;
		}

		public Rule Rule
		{
			get;
			private set;
		}

		public string Severity
		{
			get;
			private set;
		}

		public long Position
		{
			get { return Log.Position; }
		}

		public Property[] Properties
		{
			get;
			private set;
		}

		

		public Event(Log Log,Rule Rule, params Property[] Properties)
		{
			if (Properties == null) throw new ArgumentNullException("Properties");
			this.Log = Log;this.Rule = Rule;this.Properties = Properties;
			Severity = null;
			if (this.Rule.SeverityToken != null) Severity = GetValue(Rule.SeverityToken);
		}

		public Property GetProperty(string Name)
		{
			return Properties.FirstOrDefault(item => item.Name == Name);
		}
		public string GetValue(string Name)
		{
			return Properties.FirstOrDefault(item => item.Name == Name)?.Value;
		}
		

	}
}
