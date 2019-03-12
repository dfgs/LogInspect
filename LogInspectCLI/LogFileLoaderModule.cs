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
using System.Threading.Tasks;

namespace LogInspectCLI
{
	public class LogFileLoaderModule:Module
	{
		private IRegexBuilder regexBuilder;
		private FileStream stream;

		public LogFileLoaderModule(ILogger Logger, IRegexBuilder RegexBuilder) : base(Logger)
		{
			AssertParameterNotNull("RegexBuilder", RegexBuilder);
			this.regexBuilder = RegexBuilder;
		}

		public  void Load(LogFile LogFile)
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

			discardLineMatcher = LogFile.FormatHandler.CreateDiscardLinesMatcher(regexBuilder);
			appendLineToPreviousMatcher = LogFile.FormatHandler.CreateAppendLineToPreviousMatcher(regexBuilder);
			appendLineToNextMatcher = LogFile.FormatHandler.CreateAppendLineToNextMatcher(regexBuilder);

			logParser = LogFile.FormatHandler.CreateLogParser(regexBuilder);


			Log(LogLevels.Information, $"Open file name {LogFile.FileName}");
			if (!Try(() => new FileStream(LogFile.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)).OrAlert(out stream, "Failed to open file")) return;

			using (stream)
			{
				lineReader = new LineReader(stream, Encoding.Default, discardLineMatcher);
				logReader = new LogReader(lineReader, appendLineToPreviousMatcher, appendLineToNextMatcher);
				while ( logReader.CanRead)
				{
					if (!Try(() => logReader.Read()).OrAlert(out log, "Error occured while reading log")) break;
					if (!Try(() => logParser.Parse(log)).OrAlert(out ev, $"Failed to parse log at line {log.LineIndex}")) continue;
					if (ev != null) LogFile.Events.Add(ev);
				}
			}



		}



	}
}
