using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LogInspectLib
{
    public class LogReader : TextReader
    {
		private Stream stream;
		private Encoding encoding;
		private static int bufferSize = 32;
		private char[] buffer;
		private int bufferPos;
		private long position;

		private string line;
		private long linePos;

		public FormatHandler FormatHandler { get; }


		private List<Regex> appendToNextRegexes;
		private List<Regex> appendToPreviousRegexes;
		private List<LogParser> logParsers;

		public bool EndOfStream
		{
			get { return position == stream.Length; }
		}

		public LogReader(Stream Stream, Encoding Encoding, FormatHandler FormatHandler):base()
        {
			this.stream = Stream;
			this.FormatHandler = FormatHandler;
			this.encoding = Encoding;

			this.position = 0;

			this.appendToNextRegexes = new List<Regex>();
			this.appendToPreviousRegexes = new List<Regex>();
			this.logParsers = new List<LogParser>();
			
			foreach (string pattern in FormatHandler.AppendToPreviousPatterns)
			{
				this.appendToPreviousRegexes.Add(new Regex(pattern));
			}
			foreach (string pattern in FormatHandler.AppendToNextPatterns)
			{
				this.appendToNextRegexes.Add(new Regex(pattern));
			}
			foreach(Rule rule in FormatHandler.Rules)
			{
				this.logParsers.Add(new LogParser(rule));
			}
		}
		private void LoadBuffer()
		{
			byte[] data;
			int count;

			data = new byte[bufferSize];
			count=stream.Read(data, 0, bufferSize);
			if (count <=0) throw (new EndOfStreamException());
			else buffer = encoding.GetChars(data, 0, count);
			bufferPos = 0;
		}
		private char ReadBuffer()
		{
			char c;
			if ((buffer == null) || (bufferPos == buffer.Length)) LoadBuffer();
			c=buffer[bufferPos];
			bufferPos++;
			return c;
		}
		public void Seek(long Position)
		{
			buffer = null;
			position = Position;
			stream.Seek(position, SeekOrigin.Begin);
		}
		
		public override string ReadLine()
		{
			char? c;
			StringBuilder sb;


			sb = new StringBuilder(1024);

			do
			{
				c = ReadBuffer();
				position++;
				if (c == '\n') break;
				if (c == '\r') continue;
				sb.Append(c);
			} while (!EndOfStream);

			return sb.ToString();
		}


		private bool MustAppendToNextLine(string Line)
		{
			if (Line == null) return false;
			foreach (Regex regex in appendToNextRegexes)
			{
				if (regex.Match(Line).Success) return true;
			}
			return false; 
		}
		private bool MustAppendToPreviousLine(string Line)
		{
			if (Line == null) return false;
			foreach (Regex regex in appendToPreviousRegexes)
			{
				if (regex.Match(Line).Success) return true;
			}
			return false;
		}
		

		public Log ReadLog()
        {
  			List<string> lines;
 			bool mustAppend;
			Log log;
			long firstPos;


			firstPos = linePos;

			lines = new List<string>();

			if (line == null)
			{
				linePos = position;
				line = ReadLine();
			}

			do
			{
				lines.Add(line);
				mustAppend = MustAppendToNextLine(line);

				if (EndOfStream)
				{
					line = null;
					break;
					}
				linePos = position;
				line = ReadLine();
			} while (mustAppend);

			mustAppend = MustAppendToPreviousLine(line);
			while (mustAppend)
			{
				lines.Add(line);

				if (EndOfStream)
				{
					line = null;
					break;
				}

				linePos = position;
				line = ReadLine();
				mustAppend = MustAppendToPreviousLine(line);
			}

			log =new Log(lines, firstPos);
			return log;
			
        }

		public Event ReadEvent()
		{
			Log log;
			Event ev;

			log = ReadLog();
			if (log == null) return null;

			foreach(LogParser parser in logParsers)
			{
				ev = parser.Parse(log);
				if (ev!=null) return ev;
			}

			ev = new Event();
			ev.Log = log;
			return ev;
		}



    }
}
