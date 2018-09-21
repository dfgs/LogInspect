﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLib.Loaders
{
	public class LineLoader:Loader<Line>,ILineLoader
	{
		private StreamReader reader;
		private IStringMatcher discardMatcher;
		private int index;

		public override bool CanLoad
		{
			get { return reader.BaseStream.Position < reader.BaseStream.Length; }
		}

		public LineLoader(Stream Stream,Encoding Encoding,IStringMatcher DiscardMatcher)
		{
			if (Stream == null) throw new ArgumentNullException("Stream");
			if (Encoding == null) throw new ArgumentNullException("Encoding");
			if (DiscardMatcher == null) throw new ArgumentNullException("DiscardMatcher");
			this.reader = new StreamReader(Stream,Encoding);
			this.discardMatcher = DiscardMatcher;
		}

		protected override Line OnLoad()
		{
			Line item;
			item = new Line();

			do
			{
				item.Position = reader.BaseStream.Position;
				item.Value = reader.ReadLine();
				item.Index = index;
				index++;	// must use a local index in order to take discarded lines in account
				if (item.Value == null) throw new EndOfStreamException();
			} while (discardMatcher.Match(item.Value));

			return item;
		}

	}
}
