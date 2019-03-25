using LogInspect.BaseLib.Parsers;
using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules.Test.Mocks
{
	public class MockedLogParser : ILogParser
	{
		public Event Parse(Log Log)
		{
			Event ev;

			ev = new Event();
			ev.Properties["Message"] = Log.ToSingleLine();

			return ev;
		}


	}
}
