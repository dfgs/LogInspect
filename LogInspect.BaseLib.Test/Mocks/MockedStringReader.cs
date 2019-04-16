using LogInspect.BaseLib.Readers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LogInspect.BaseLib.Test.Mocks
{
	public class MockedStringReader : IStringReader
	{
		int index;
		int max;

		public MockedStringReader(int Max)
		{
			this.max = Max;
		}
		public string Read()
		{
			if (index == max) return null;
			return $"Line {index++}";
		}
	}
}
