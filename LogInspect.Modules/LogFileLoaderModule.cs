using LogInspect.BaseLib;
using LogInspect.BaseLib.Builders;
using LogInspect.BaseLib.Parsers;
using LogInspect.Models;
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
	public class LogFileLoaderModule : Module, ILogFileLoaderModule
	{
		private ILineReader lineReader;
		private ILineBuilder lineBuilder;
		private ILogBuilder logBuilder;
		private ILogParser logParser;
		private IEventList eventList;
		

		public LogFileLoaderModule(ILogger Logger, ILineReader LineReader, ILineBuilder LineBuilder, ILogBuilder LogBuilder, ILogParser LogParser,IEventList EventList) : base(Logger)
		{
			AssertParameterNotNull(LineReader, "LineReader", out lineReader);
			AssertParameterNotNull(LineBuilder, "LineBuilder", out lineBuilder);
			AssertParameterNotNull(LogBuilder, "LogBuilder", out logBuilder);
			AssertParameterNotNull(LogParser, "LogParser", out logParser);
			AssertParameterNotNull(EventList, "EventList", out eventList);
		}

		

		private IStringMatcher CreateStringMatcher(IRegexBuilder RegexBuilder,string NameSpace, IEnumerable<string> Patterns)
		{
			StringMatcher matcher;

			matcher = new StringMatcher();
			foreach (string pattern in Patterns)
			{
				matcher.Add(RegexBuilder.Build(NameSpace, pattern, false));
			}
			return matcher;
		}

		public void Load()
		{
			string l;
			Line line;
			Log log;
			Event ev;


			while (!lineReader.EOF)
			{ 
				try
				{
					l = lineReader.Read();
					if (!lineBuilder.Push(l, out line)) continue;
					if (!logBuilder.Push(line, out log)) continue;
				}
				catch (Exception ex)
				{
					this.Log(ex);
					break;
				}
				if (!Try(() => logParser.Parse(log)).OrAlert(out ev, $"Failed to parse log at line {log.LineIndex}")) continue;
				if (ev == null) continue;

				//Console.WriteLine(string.Join("	", ev.Properties));
				if (!Try(()=>eventList.Add(ev)).OrAlert("Failed to add event")) continue;
			}

		}





	}
}
