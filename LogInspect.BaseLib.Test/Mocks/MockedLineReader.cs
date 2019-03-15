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
	public class MockedLineReader : Reader<Line>, ILineReader
	{
		public override bool CanRead
		{
			get { return count != max; }
		}

		private int count;
		private int max ;
		public MockedLineReader(int Max=-1)
		{
			this.max = Max;
		}
		protected override Line OnRead()
		{
			Line result;

			if (!CanRead) throw new EndOfStreamException();
			result = new Line() { Index = count, Value = $"Item {count}" };
			count++;
			return result;
		}
	}
}
