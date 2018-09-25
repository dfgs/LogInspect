using LogInspectLib;
using LogInspectLib.Readers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLibTest.Mocks
{
	public class MockedLineReaderWithIncompleteAppend : Reader<Line>, ILineReader
	{

		private bool canRead;
		public override bool CanRead
		{
			get { return canRead; }
		}

		public MockedLineReaderWithIncompleteAppend()
		{
			canRead = true;
		}
		protected override Line OnRead()
		{
			Line result;

			if (!CanRead) throw new EndOfStreamException();
			result = new Line() { Index = 0, Value = $"Item " };
			canRead = false;
			return result;
		}
	}
}
