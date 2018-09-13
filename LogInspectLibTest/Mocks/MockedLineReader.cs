using LogInspectLib;
using LogInspectLib.Readers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLibTest.Mocks
{
	public class MockedLineReader : ILineReader
	{
		public bool EndOfStream => throw new NotImplementedException();

		public long Position => throw new NotImplementedException();

		public long Length => throw new NotImplementedException();

		public Line Read()
		{
			throw new NotImplementedException();
		}

		public void Seek(long Position)
		{
			throw new NotImplementedException();
		}
	}
}
