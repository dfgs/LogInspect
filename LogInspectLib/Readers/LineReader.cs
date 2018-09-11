using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspectLib.Readers
{
    public class LineReader:Reader<Line>
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
			long availableBytes;

			pos = Position;
			sb = new StringBuilder(2048);

			do
			{ 
				availableBytes = charReader.AvailableBytes;	// needed optimization because charReader.EndOfStream is very slow
				for (long t = 0; t < availableBytes; t++)
				{
					c = charReader.Read();
					if (c == '\n') goto result;
					if (c == '\r') continue;
					sb.Append(c);
				}
			} while (!charReader.EndOfStream);
			result:
			return new Line(pos, sb.ToString());

		}

	



	}
}
