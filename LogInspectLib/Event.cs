using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLib
{
	public class Event
	{
		public string this[string Name]
		{
			get { return Properties[Name]; }
			set { Properties[Name] = value; }
		}

		public int LineIndex
		{
			get;
			set;
		}
		/*public Log Log
		{
			get;
			set;
		}

		public Rule Rule
		{
			get;
			set;
		}

		public long Position
		{
			get { return Log?.Position??-1; }
		}*/

		public PropertyCollection<string> Properties
		{
			get;
			private set;
		}

		public Event()
		{
			this.Properties = new PropertyCollection<string>();
		}

		


	}
}
