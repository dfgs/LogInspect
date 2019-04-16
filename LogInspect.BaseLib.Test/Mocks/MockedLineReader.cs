using LogInspect.BaseLib.Readers;
using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LogInspect.BaseLib.Test.Mocks
{
	public class MockedLineReader : ILineReader
	{
		int index;
		int max;


		public MockedLineReader(int Max)
		{
			this.max = Max;
		}
		public Line Read()
		{
			if (index == max) return null;
			return new Line() { Index=index,Value= $"Line {index++}" };
		}

	}
}
