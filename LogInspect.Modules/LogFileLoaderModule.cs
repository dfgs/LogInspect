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
		
		

		public LogFileLoaderModule(ILogger Logger, ILineReader LineReader, ILineBuilder LineBuilder, ILogBuilder LogBuilder, ILogParser LogParser) : base(Logger)
		{
			AssertParameterNotNull(LineReader, "LineReader", out lineReader);
			AssertParameterNotNull(LineBuilder, "LineBuilder", out lineBuilder);
			AssertParameterNotNull(LogBuilder, "LogBuilder", out logBuilder);
			AssertParameterNotNull(LogParser, "LogParser", out logParser);
		}

		


		public IEnumerable<Event> Load()
		{
			string l;
			Line line;
			Log log;
			Event ev;


			while (true)
			{ 
				try
				{
					if (!lineReader.EOF)
					{
						l = lineReader.Read();
						if (!lineBuilder.Push(l, out line)) continue;
						if (!logBuilder.Push(line, out log)) continue;
					}
					else if (lineBuilder.CanFlush)
					{
						line = lineBuilder.Flush();
						if (!logBuilder.Push(line, out log)) continue;
					}
					else if (logBuilder.CanFlush)
					{
						log = logBuilder.Flush();
					}
					else yield break;

				}
				catch (Exception ex)
				{
					this.Log(ex);
					break;
				}
				if (!Try(() => logParser.Parse(log)).OrAlert(out ev, $"Failed to parse log at line {log.LineIndex}")) continue;
				if (ev == null) continue;

				yield return ev;
			}

			
		}





	}
}
