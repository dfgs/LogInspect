using LogInspect.Models;
using LogInspectLib;
using LogInspectLib.Parsers;
using LogInspectLib.Readers;
using LogLib;
using ModuleLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogInspect
{
	public class LogFileLoaderModule : ThreadModule, ILogFileLoaderModule
	{
		private LogFile logFile;
		private IRegexBuilder regexBuilder;
		private FormatHandler formatHandler;

		public LogFileLoaderModule(ILogger Logger, LogFile LogFile, IRegexBuilder RegexBuilder, FormatHandler FormatHandler) : base(Logger)
		{
			AssertParameterNotNull("LogFile", LogFile);
			AssertParameterNotNull("RegexBuilder", RegexBuilder);
			AssertParameterNotNull("FormatHandler", FormatHandler);
			this.logFile = LogFile;
			this.regexBuilder = RegexBuilder;
			this.formatHandler = FormatHandler;
		}

		protected override void ThreadLoop()
		{
			FileStream stream;
			IStringMatcher discardLineMatcher;
			IStringMatcher appendLineToPreviousMatcher;
			IStringMatcher appendLineToNextMatcher;
			ILogParser logParser;
			ILineReader lineReader;
			ILogReader logReader;

			Log(LogLevels.Information, $"Open file name {logFile.FileName}");
			if (!Try(() => new FileStream(logFile.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)).OrAlert(out stream, "Failed to open file")) return;

			using (stream)
			{

				discardLineMatcher = formatHandler.CreateDiscardLinesMatcher(regexBuilder);
				appendLineToPreviousMatcher = formatHandler.CreateAppendLineToPreviousMatcher(regexBuilder);
				appendLineToNextMatcher = formatHandler.CreateAppendLineToNextMatcher(regexBuilder);

				logParser = formatHandler.CreateLogParser(regexBuilder);

				lineReader = new LineReader(stream, Encoding.Default, discardLineMatcher);
				logReader = new LogReader(lineReader, appendLineToPreviousMatcher, appendLineToNextMatcher);

			}



			Thread.Sleep(5000);
		}
	}
}
