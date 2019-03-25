using LogInspect.BaseLib.Builders;
using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules.Test.Mocks
{
	public class MockedLogBuilder : ILogBuilder
	{
		public bool CanFlush => false;

		public Log Flush()
		{
			throw new NotImplementedException();
		}

		public bool Push(Line Input, out Log Output)
		{
			Output = new Log();Output.Lines.Add(Input);
			return true;
		}

	}
}
