using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspectLib.Readers
{
    public class LineReader:Reader<Line>,ILineReader
    {

		public override long Position
		{
			get { return charReader.Position; }
		}
		public override long Length
		{
			get { return charReader.Length; }
		}

		public override bool EndOfStream
		{
			get { return charReader.EndOfStream; }
		}

		private ICharReader charReader;
		
		public LineReader(ICharReader CharReader):base()
        {
			if (CharReader == null) throw new ArgumentNullException("CharReader");

			this.charReader = CharReader;
		}


		protected override void OnSeek(long Position)
		{
			charReader.Seek(Position);
		}

		protected override Line OnRead()
		{
			char c;
			StringBuilder sb;
			long pos;
			long availableBytes;

			pos = Position;
			sb = new StringBuilder(2048);

			if (charReader.EndOfStream) throw new EndOfStreamException();

			while (!charReader.EndOfStream)
			{
				availableBytes = charReader.AvailableBytes;	// needed optimization because charReader.EndOfStream is very slow
				for (long t = 0; t < availableBytes; t++)
				{
					c = charReader.Read();
					if (c == '\n') goto result;
					if (c == '\r') continue;
					sb.Append(c);
				}
			} 
			result:
			return new Line() { Position = pos, Value = sb.ToString() };

		}

	



	}
}
