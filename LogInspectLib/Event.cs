using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLib
{
	public class Event
	{
		public Log Log
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
			get { return Log.Position; }
		}

		public List<Property> Properties
		{
			get;
			set;
		}

		public Event()
		{
			Properties = new List<Property>();
		}

	}
}
