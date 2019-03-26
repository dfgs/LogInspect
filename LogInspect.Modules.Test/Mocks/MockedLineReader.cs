using LogInspect.BaseLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules.Test.Mocks
{
	public class MockedLineReader : ILineReader
	{
		private int index;

		public bool EOF => index>=count;

		public int count;

		public MockedLineReader(int Count)
		{
			this.count = Count;
		}

		public string Read()
		{
			string line;

			line=$"Line {index}";
			index++;
			return line;
		}

	}
}
