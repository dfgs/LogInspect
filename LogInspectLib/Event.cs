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

		public Event(Log Log,Rule Rule,params Property[] Properties)
		{
			//if (Rule == null) throw new ArgumentNullException("Rule");
			if (Properties == null) throw new ArgumentNullException("Properties");
			this.Log = Log;this.Rule = Rule;this.Properties = Properties;
			
		}

		public Property GetProperty(string Name)
		{
			return Properties.FirstOrDefault(item => item.Name == Name);
		}

		

	}
}
