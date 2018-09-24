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
	public class MockedLineReaderWithAppend : Reader<Line>, ILineReader
	{
		private int count;
		private int max ;

		public override bool CanRead
		{
			get { return count != max; }
		}

		public MockedLineReaderWithAppend(int Max=-1)
		{
			this.max = Max;
		}
		protected override Line OnRead()
		{
			Line result;

			if (!CanRead) throw new EndOfStreamException();
			if ((count & 1)==0) result = new Line() { Index = count, Value = $"Item " };
			else result = new Line() { Index = count, Value = $"{count}" }; ;
			count++;
			return result;
		}
	}
}
