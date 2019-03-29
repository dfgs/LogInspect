using LogInspect.BaseLib.Builders;
using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules.Test.Mocks
{
	public class MockedLogBuilderWithFlush : ILogBuilder
	{
		private Log log = null;

		public bool CanFlush => log != null;

		public Log Flush()
		{
			Log result;
			result = log;
			log = null;
			return result;
		}

		public bool Push(Line Input, out Log Output)
		{
			Output = log;
			log = new Log();
			log.Lines.Add(Input);

			return Output != null;
		}


	}
}
