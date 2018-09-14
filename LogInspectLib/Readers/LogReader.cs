using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspectLib.Readers
{
    public class LogReader :Reader<Log>,ILogReader
    {
		private int readLines;

		public override bool EndOfStream
		{
			get { return lineReader.EndOfStream; }
		}

		public override long Position
		{
			get { return lineReader.Position; }
		}
		public override long Length
		{
			get { return lineReader.Length; }
		}


		private ILineReader lineReader;


		//public FormatHandler FormatHandler { get; }

		private List<Regex> appendToNextRegexes;
		private List<Regex> appendToPreviousRegexes;
		private List<Regex> discardRegexes;
		
		public LogReader(ILineReader LineReader, IRegexBuilder RegexBuilder,string DefaultNameSpace, IEnumerable<string> AppendLineToPreviousPatterns, IEnumerable<string> AppendLineToNextPatterns, IEnumerable<string> DiscardLinePatterns) :base()
        {
			if (LineReader == null) throw new ArgumentNullException("LineReader");
			if (RegexBuilder == null) throw new ArgumentNullException("RegexBuilder");

			this.lineReader = LineReader;// new LineReader(new CharReader( Stream, Encoding, BufferSize));

			//this.FormatHandler = FormatHandler;
			
			this.appendToNextRegexes = new List<Regex>();
			this.appendToPreviousRegexes = new List<Regex>();
			this.discardRegexes = new List<Regex>();

			foreach (string pattern in AppendLineToPreviousPatterns ?? Enumerable.Empty<string>())
			{
				this.appendToPreviousRegexes.Add(RegexBuilder.Build(DefaultNameSpace, pattern));
			}
			foreach (string pattern in AppendLineToNextPatterns ?? Enumerable.Empty<string>())
			{
				this.appendToNextRegexes.Add(RegexBuilder.Build(DefaultNameSpace, pattern));
			}
			foreach (string pattern in DiscardLinePatterns ?? Enumerable.Empty<string>())
			{
				this.discardRegexes.Add(RegexBuilder.Build(DefaultNameSpace,pattern));
			}

		}
		protected override void OnSeek(long Position)
		{
			lineReader.Seek(Position);
		}


		private bool MustAppendToNextLine(Line Line)
		{
			//return false;
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
		private bool MustDiscardLine(Line Line)
		{
			foreach (Regex regex in discardRegexes)
			{
				if (regex.Match(Line.Value).Success) return true;
			}
			return false;
		}

		public int GetReadLines()
		{
			return readLines;
		}

		protected override Log OnRead()
        {
			Line line;
 			bool mustAppend;
			Log log;
			long pos;

			log = new Log();

			readLines = 0;
			do
			{
				line = lineReader.Read();readLines++;
				if (MustDiscardLine(line))
				{
					mustAppend = true;
				}
				else
				{
					log.Lines.Add(line);
					mustAppend = MustAppendToNextLine(line);
				}
			} while ((mustAppend)&&(!EndOfStream));

			while (!EndOfStream) 
			{
				pos = lineReader.Position;
				line = lineReader.Read(); readLines++;
				if (!MustDiscardLine(line))
				{
					mustAppend = MustAppendToPreviousLine(line);
					if (mustAppend)
					{
						log.Lines.Add(line);
					}
					else
					{
						lineReader.Seek(pos);
						readLines--;
						break;
					}
				}
			}

			return log;
			
        }

		



	}
}
