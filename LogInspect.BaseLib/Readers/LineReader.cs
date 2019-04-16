using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogInspect.Models;

namespace LogInspect.BaseLib.Readers
{
	public class LineReader : ILineReader
	{
		private IStringReader stringReader;
		private IStringMatcher discardMatcher;
		private int index;

		public LineReader(IStringReader StringReader,IStringMatcher DiscardMatcher)
		{
			if (DiscardMatcher == null) throw new ArgumentNullException("DiscardMatcher");
			if (StringReader == null) throw new ArgumentNullException("StringReader");
			this.discardMatcher = DiscardMatcher;
			this.stringReader = StringReader;
			
		}

		public Line Read()
		{
			string value;

			do
			{
				value = stringReader.Read();
				if (value == null) return null;
				index++;
			} while (discardMatcher.Match(value));
			
			return new Line() { Value = value, Index = index-1 };
		}


	}
}
