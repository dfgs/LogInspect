using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Models.Readers
{
	public class LineReader:Reader<Line>,ILineReader
	{
		private StreamReader reader;
		private IStringMatcher discardMatcher;
		private int index;

		public override bool CanRead
		{
			get { return !reader.EndOfStream; }
		}

		public LineReader(Stream Stream, Encoding Encoding, IStringMatcher DiscardMatcher)
		{
			if (Stream == null) throw new ArgumentNullException("Stream");
			if (Encoding == null) throw new ArgumentNullException("Encoding");
			if (DiscardMatcher == null) throw new ArgumentNullException("DiscardMatcher");
			this.reader = new StreamReader(Stream, Encoding);
			this.discardMatcher = DiscardMatcher;
		}

		protected override Line OnRead()
		{
			Line item;
			item = new Line();

			do
			{
				item.Position = reader.BaseStream.Position;
				item.Value = reader.ReadLine();
				item.Index = index;
				index++;    // must use a local index in order to take discarded lines in account
				if (item.Value == null) throw new EndOfStreamException();
			} while (discardMatcher.Match(item.Value));

			return item;
		}


	}
}
