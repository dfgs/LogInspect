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
using System.Threading.Tasks;

namespace LogInspectCLI
{
	public class LogFileLoaderModule:Module
	{
		private IRegexBuilder regexBuilder;
		private FileStream stream;

		public LogFileLoaderModule(ILogger Logger, IRegexBuilder RegexBuilder) : base(Logger)
		{
			AssertParameterNotNull(RegexBuilder,"RegexBuilder", out regexBuilder);
			this.regexBuilder = RegexBuilder;
		}

		private IStringMatcher CreateStringMatcher(IRegexBuilder RegexBuilder, string NameSpace, IEnumerable<string> Patterns)
		{
			IStringMatcher matcher;

			matcher = new StringMatcher();
			foreach (string pattern in Patterns)
			{
				matcher.Add(RegexBuilder.Build(NameSpace, pattern, false));
			}
			return matcher;
		}
		public void Load(LogFile LogFile)
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

			discardLineMatcher = CreateStringMatcher(regexBuilder, LogFile.FormatHandler.NameSpace, LogFile.FormatHandler.DiscardLinePatterns);
			appendLineToPreviousMatcher = CreateStringMatcher(regexBuilder, LogFile.FormatHandler.NameSpace, LogFile.FormatHandler.AppendLineToPreviousPatterns);
			appendLineToNextMatcher = CreateStringMatcher(regexBuilder, LogFile.FormatHandler.NameSpace, LogFile.FormatHandler.AppendLineToNextPatterns);


			logParser = new LogParser(Logger, LogFile.FormatHandler.Columns.Select(item => item.Name));
			foreach (Rule rule in LogFile.FormatHandler.Rules)
			{
				logParser.Add(regexBuilder.Build(LogFile.FormatHandler.NameSpace, rule.GetPattern(), false), rule.Discard);
			}


			Log(LogLevels.Information, $"Open file name {LogFile.FileName}");
			if (!Try(() => new FileStream(LogFile.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)).OrAlert(out stream, "Failed to open file")) return;

			using (stream)
			{
				lineReader = new LineReader(Logger, stream, Encoding.Default, discardLineMatcher);
				logReader = new LogReader(Logger, lineReader, appendLineToPreviousMatcher, appendLineToNextMatcher);
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
