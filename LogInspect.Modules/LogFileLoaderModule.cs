using LogInspect.Models;
using LogInspect.Modules.Parsers;
using LogInspect.Modules.Readers;
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
			AssertParameterNotNull(LogFile,"LogFile", out logFile);
			AssertParameterNotNull(RegexBuilder,"RegexBuilder", out regexBuilder);
		}


		private IStringMatcher CreateStringMatcher(IRegexBuilder RegexBuilder,string NameSpace, IEnumerable<string> Patterns)
		{
			IStringMatcher matcher;

			matcher = new StringMatcher();
			foreach (string pattern in Patterns)
			{
				matcher.Add(RegexBuilder.Build(NameSpace, pattern, false));
			}
			return matcher;
		}

		protected override void ThreadLoop()
		{
			IStringMatcher discardLineMatcher;
			IStringMatcher appendLineToPreviousMatcher;
			IStringMatcher appendLineToNextMatcher;
			LogParser logParser;
			ILineReader lineReader;
			ILogReader logReader;
			Log log;
			Event ev;

			LogEnter();

			discardLineMatcher = CreateStringMatcher(regexBuilder, logFile.FormatHandler.NameSpace, logFile.FormatHandler.DiscardLinePatterns);
			appendLineToPreviousMatcher = CreateStringMatcher(regexBuilder, logFile.FormatHandler.NameSpace, logFile.FormatHandler.AppendLineToPreviousPatterns);
			appendLineToNextMatcher = CreateStringMatcher(regexBuilder, logFile.FormatHandler.NameSpace, logFile.FormatHandler.AppendLineToNextPatterns);


			logParser = new LogParser(Logger,logFile.FormatHandler.Columns.Select(item => item.Name));
			foreach (Rule rule in logFile.FormatHandler.Rules)
			{
				logParser.Add(regexBuilder.Build(logFile.FormatHandler.NameSpace, rule.GetPattern(), false), rule.Discard);
			}


			Log(LogLevels.Information, $"Open file name {logFile.FileName}");
			if (!Try(() => new FileStream(logFile.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)).OrAlert(out stream, "Failed to open file")) return;
			Length = stream.Length;

			using (stream)
			{
				lineReader = new LineReader(Logger, stream, Encoding.Default, discardLineMatcher);
				logReader = new LogReader(Logger, lineReader, appendLineToPreviousMatcher, appendLineToNextMatcher);
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
