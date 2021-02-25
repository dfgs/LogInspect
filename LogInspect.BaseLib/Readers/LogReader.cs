using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogInspect.Models;

namespace LogInspect.BaseLib.Readers
{
	public class LogReader:ILogReader
	{
		private IStringMatcher logPrefixMatcher;
		private IStringMatcher discardLogMatcher;
		private ILineReader lineReader;
		private Line previousLine;

		public LogReader(ILineReader LineReader, IStringMatcher LogPrefixMatcher, IStringMatcher DiscardLogMatcher)
		{
			if (LogPrefixMatcher == null) throw new ArgumentNullException("LogPrefixMatcher");
			if (LineReader == null) throw new ArgumentNullException("LineReader");
			if (DiscardLogMatcher == null) throw new ArgumentNullException("DiscardLogMatcher");
			this.lineReader = LineReader;
			this.logPrefixMatcher = LogPrefixMatcher;
			this.discardLogMatcher = DiscardLogMatcher;
		}

		public Log Read()
		{
			Log log;

			if (previousLine == null) previousLine = lineReader.Read();
			if (previousLine == null) return null;

			do
			{
				log = new Log();
				log.Lines.Add(previousLine);

				while (true)
				{
					previousLine = lineReader.Read();
					if (previousLine == null) break;
					if (logPrefixMatcher.Match(previousLine.Value)) break;
					log.Lines.Add(previousLine);
				}

				if (discardLogMatcher.Match(log.ToSingleLine()))
					log=null;	// me must restart loading of a new log
			} while ((previousLine!=null) && (log==null));

			return log;

		}


	}
}
