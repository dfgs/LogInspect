using LogInspect.Models;
using LogLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules.Readers
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

		public LineReader(ILogger Logger,Stream Stream, Encoding Encoding, IStringMatcher DiscardMatcher):base(Logger)
		{

			AssertParameterNotNull(DiscardMatcher, "DiscardMatcher", out discardMatcher);
			AssertParameterNotNull(Encoding, "Encoding", out Encoding encoding);
			AssertParameterNotNull(Stream, "Stream", out Stream stream);
			this.reader = new StreamReader(stream, encoding);
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
