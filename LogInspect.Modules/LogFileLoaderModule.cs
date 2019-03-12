using LogInspect.Models;
using LogInspect.Models.Parsers;
using LogInspect.Models.Readers;
using LogLib;
using ModuleLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public class LogFileLoaderModule : ThreadModule, ILogFileLoaderModule
	{
		private LogFile logFile;
		private IRegexBuilder regexBuilder;
		private FileStream stream;

		public long Position
		{
			get;
			private set;
		}
		public long Length
		{
			get;
			private set;
		}

		public int Count
		{
			get { return logFile.Events.Count; }
		}

		public LogFileLoaderModule(ILogger Logger, LogFile LogFile, IRegexBuilder RegexBuilder) : base(Logger)
		{
			AssertParameterNotNull("LogFile", LogFile);
			AssertParameterNotNull("RegexBuilder", RegexBuilder);
			this.logFile = LogFile;
			this.regexBuilder = RegexBuilder;
		}

		protected override void ThreadLoop()
		{
			IStringMatcher discardLineMatcher;
			IStringMatcher appendLineToPreviousMatcher;
			IStringMatcher appendLineToNextMatcher;
			ILogParser logParser;
			ILineReader lineReader;
			ILogReader logReader;
			Log log;
			Event ev;

			LogEnter();

			discardLineMatcher = logFile.FormatHandler.CreateDiscardLinesMatcher(regexBuilder);
			appendLineToPreviousMatcher = logFile.FormatHandler.CreateAppendLineToPreviousMatcher(regexBuilder);
			appendLineToNextMatcher = logFile.FormatHandler.CreateAppendLineToNextMatcher(regexBuilder);

			logParser = logFile.FormatHandler.CreateLogParser(regexBuilder);


			Log(LogLevels.Information, $"Open file name {logFile.FileName}");
			if (!Try(() => new FileStream(logFile.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)).OrAlert(out stream, "Failed to open file")) return;
			Length = stream.Length;

			using (stream)
			{
				lineReader = new LineReader(stream, Encoding.Default, discardLineMatcher);
				logReader = new LogReader(lineReader, appendLineToPreviousMatcher, appendLineToNextMatcher);
				while((State==ModuleStates.Started) && logReader.CanRead)
				{
					Position = stream.Position;
					if (!Try(() => logReader.Read()).OrAlert(out log, "Error occured while reading log")) break;
					if (!Try(() => logParser.Parse(log)).OrAlert(out ev, $"Failed to parse log at line {log.LineIndex}")) continue;
					if (ev!=null) logFile.Events.Add(ev);
				}
			}
			


		}
	}
}
