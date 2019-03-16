using LogInspect.BaseLib.Readers;
using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.BaseLib.Test.Mocks
{
	public class MockedLogReader : ILogReader
	{
		private int count;
		public int index;
		public bool CanRead => index<count;

		public MockedLogReader(int Count)
		{
			this.count = Count;
			this.index = 0;
		}

		public Log Read()
		{
			Log log;
			if (index == count ) throw new EndOfStreamException("End of stream");
			log = new Log();
			log.Lines.Add(new Line() { Index = index, Position = 0, Value = $"Line{index}" });
			index++;
			return log;
		}


	}
}
