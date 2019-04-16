using LogInspect.BaseLib.Readers;
using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules.Test.Mocks
{
	public class MockedLogReader : ILogReader
	{
		private int index;
		private int max;

		public MockedLogReader(int Max)
		{
			this.max = Max;
		}

		public Log Read()
		{
			Log log;

			if (index == max) return null;
			log = new Log();
			log.Lines.Add(new Line() { Index = index,Value=$"Line {index}" });
			index++;
			return log;
		}
	}
}
