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
		/*public int Index
		{
			get { return Log.Index; }
		}*/
		public long Position
		{
			get { return Log.Position; }
		}

		public Property[] Properties
		{
			get;
			private set;
		}

		public Severity Severity
		{
			get;
			private set;
		}

		public Event(Log Log,Rule Rule, Severity Severity,params Property[] Properties)
		{
			if (Properties == null) throw new ArgumentNullException("Properties");
			this.Log = Log;this.Rule = Rule;this.Severity = Severity; this.Properties = Properties;
			
		}

		public Property GetProperty(string Name)
		{
			return Properties.FirstOrDefault(item => item.Name == Name);
		}
		public object GetValue(string Name)
		{
			return Properties.FirstOrDefault(item => item.Name == Name)?.Value;
		}
		

	}
}
