using LogInspect.BaseLib;
using LogInspect.BaseLib.Parsers;
using LogInspect.BaseLib.Readers;
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
		private ILogReader logReader;
		private ILogParser logParser;
		
		

		public LogFileLoaderModule(ILogger Logger, ILogReader LogReader, ILogParser LogParser) : base(Logger)
		{
			AssertParameterNotNull(LogReader, "LogReader", out logReader);
			AssertParameterNotNull(LogParser, "LogParser", out logParser);
		}

		


		public IEnumerable<Event> Load()
		{
			Log log;
			Event ev;


			while (true)
			{ 
				try
				{
					log = logReader.Read();
				}
				catch (Exception ex)
				{
					this.Log(ex);
					break;
				}
				if (log == null) yield break;
				if (!Try(() => logParser.Parse(log)).OrAlert(out ev, $"Failed to parse log at line {log.LineIndex}")) continue;
				if (ev == null) continue;
				yield return ev;
			}

			
		}





	}
}
