using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LogInspectLib.Readers
{
    public class LogReader :Reader<Log>
    {
		
		public override bool EndOfStream
		{
			get { return lineReader.EndOfStream; }
		}

		public override long Position
		{
			get { return lineReader.Position; }
		}

		private LineReader lineReader;


		public FormatHandler FormatHandler { get; }

		private List<Regex> appendToNextRegexes;
		private List<Regex> appendToPreviousRegexes;

		
		public LogReader(Stream Stream, Encoding Encoding,int BufferSize, FormatHandler FormatHandler):base()
        {
			if (Stream == null) throw new ArgumentNullException("Stream");
			if (Encoding == null) throw new ArgumentNullException("Encoding");
			if (BufferSize <= 0) throw new ArgumentException("BufferSize");
			if (FormatHandler == null) throw new ArgumentNullException("FormatHandler");

			this.lineReader = new LineReader(Stream, Encoding, BufferSize);

			this.FormatHandler = FormatHandler;
			
			this.appendToNextRegexes = new List<Regex>();
			this.appendToPreviousRegexes = new List<Regex>();
			
			foreach (string pattern in FormatHandler.AppendToPreviousPatterns)
			{
				this.appendToPreviousRegexes.Add(new Regex(pattern));
			}
			foreach (string pattern in FormatHandler.AppendToNextPatterns)
			{
				this.appendToNextRegexes.Add(new Regex(pattern));
			}
			
		}
		protected override void OnSeek(long Position)
		{
			lineReader.Seek(Position);
		}


		private bool MustAppendToNextLine(Line Line)
		{
			foreach (Regex regex in appendToNextRegexes)
			{
				if (regex.Match(Line.Value).Success) return true;
			}
			return false; 
		}
		private bool MustAppendToPreviousLine(Line Line)
		{
			foreach (Regex regex in appendToPreviousRegexes)
			{
				if (regex.Match(Line.Value).Success) return true;
			}
			return false;
		}
		

		protected override Log OnRead()
        {
  			List<Line> lines;
			Line line;
 			bool mustAppend;
			Log log;
			long pos;

			lines = new List<Line>();
			do
			{
				line = lineReader.Read();
				lines.Add(line);
				mustAppend = MustAppendToNextLine(line);
			} while ((mustAppend)&&(!EndOfStream));

			while (!EndOfStream) 
			{
				pos = lineReader.Position;
				line = lineReader.Read();
				mustAppend = MustAppendToPreviousLine(line);
				if (mustAppend) lines.Add(line);
				else
				{
					lineReader.Seek(pos);
					break;
				}
			}

			log =new Log(lines.ToArray());
			return log;
			
        }

	


    }
}
