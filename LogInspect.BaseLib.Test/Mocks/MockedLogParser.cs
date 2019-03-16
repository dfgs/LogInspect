using LogInspect.BaseLib.Parsers;
using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.BaseLib.Test.Mocks
{
	public class MockedLogParser : ILogParser
	{
		public Event Parse(Log Log)
		{
			Event ev;
			ev = new Event();
			ev.LineIndex = Log.LineIndex;
			ev.Properties["Index"] = ev.LineIndex.ToString();
			ev.Properties["C1"] = Log.ToSingleLine();

			return ev;
		}


	}
}
