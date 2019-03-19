using LogInspect.BaseLib;
using LogInspect.BaseLib.Builders;
using LogInspect.BaseLib.FileLoaders;
using LogInspect.BaseLib.Parsers;
using LogInspect.Models;
using LogInspect.Modules;
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
	public class LogDumperModule : Module
	{
		private string fileName;
		private IRegexBuilder regexBuilder;
		private FormatHandler formatHandler;

		public LogDumperModule(ILogger Logger,string FileName,FormatHandler FormatHandler, IRegexBuilder RegexBuilder) : base(Logger)
		{
			AssertParameterNotNull(FileName, "FileName", out fileName);
			AssertParameterNotNull(FormatHandler, "FormatHandler", out formatHandler);
			AssertParameterNotNull(RegexBuilder, "RegexBuilder", out regexBuilder);
		}

		private IStringMatcher CreateStringMatcher(IRegexBuilder RegexBuilder, string NameSpace, IEnumerable<string> Patterns)
		{
			StringMatcher matcher;

			matcher = new StringMatcher();
			foreach (string pattern in Patterns)
			{
				matcher.Add(RegexBuilder.Build(NameSpace, pattern, false));
			}
			return matcher;
		}

		public void Dump(int Count)
		{
			FileStream stream;
			StreamReader reader;

			IStringMatcher discardLineMatcher;
			IStringMatcher discardLogMatcher;
			IStringMatcher appendLineToPreviousMatcher;
			IStringMatcher appendLineToNextMatcher;

			ILineBuilder lineBuilder;
			ILogBuilder logBuilder;

			LogParser logParser;

			Line line;
			Log log;
			Event ev;

			int index;

			discardLineMatcher = CreateStringMatcher(regexBuilder, formatHandler.NameSpace, formatHandler.DiscardLinePatterns);
			discardLogMatcher = CreateStringMatcher(regexBuilder, formatHandler.NameSpace,formatHandler.Rules.Where(item => item.Discard).Select(item => item.GetPattern()));
			appendLineToPreviousMatcher = CreateStringMatcher(regexBuilder, formatHandler.NameSpace, formatHandler.AppendLineToPreviousPatterns);
			appendLineToNextMatcher = CreateStringMatcher(regexBuilder, formatHandler.NameSpace, formatHandler.AppendLineToNextPatterns);

			lineBuilder = new LineBuilder(discardLineMatcher);
			logBuilder = new LogBuilder(discardLogMatcher, appendLineToPreviousMatcher, appendLineToNextMatcher);

			logParser = new LogParser(formatHandler.Columns);
			foreach (Rule rule in formatHandler.Rules.Where(item => !item.Discard))
			{
				logParser.Add(regexBuilder.Build(formatHandler.NameSpace, rule.GetPattern(), false));
			}



			Log(LogLevels.Information, $"Open file name {fileName}");
			if (!Try(() => new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)).OrAlert(out stream, "Failed to open file")) return;

			index = 0;
			using (stream)
			{
				reader = new StreamReader(stream);	
				while ((stream.Position<stream.Length) && (index<Count))
				{
					index++;
					try
					{
						if (!lineBuilder.Push(reader.ReadLine(), out line)) continue;
						if (!logBuilder.Push(line, out log)) continue;
					}
					catch(Exception ex)
					{
						this.Log(ex);
						break;
					}
					if (!Try(() => logParser.Parse(log)).OrAlert(out ev, $"Failed to parse log at line {log.LineIndex}")) continue;

					Console.WriteLine(string.Join("	", ev.Properties));

				}
			}


		}

	}
}
