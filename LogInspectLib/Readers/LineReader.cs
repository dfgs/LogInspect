using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LogInspectLib.Readers
{
    public class LineReader:Reader<Line>
    {

		public override long Position
		{
			get { return charReader.Position; }
		}
		public override bool EndOfStream
		{
			get { return charReader.EndOfStream; }
		}

		private CharReader charReader;
		
		public LineReader(Stream Stream, Encoding Encoding,int BufferSize):base()
        {
			if (Stream == null) throw new ArgumentNullException("Stream");
			if (Encoding == null) throw new ArgumentNullException("Encoding");
			if (BufferSize <= 0) throw new ArgumentException("BufferSize");

			this.charReader = new CharReader(Stream, Encoding, BufferSize);
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

			pos = Position;
			sb = new StringBuilder(1024);
			do
			{
				c = charReader.Read(); ;
				if (c == '\n') break;
				if (c == '\r') continue;
				sb.Append(c);
			} while (!charReader.EndOfStream);

			return new Line(pos, sb.ToString());

		}


		


    }
}
