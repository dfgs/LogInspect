using LogInspectLib;
using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect
{
	class Program
	{
		static void Main(string[] args)
		{
			ILogger logger;

			LogFileManager schemaManager;
			//Log log;
			Event ev;
			LogInspectLib.Rule rule;

			logger = new ConsoleLogger(new DefaultLogFormatter());

			

			LogReader reader = schemaManager.CreateLogReader(@"E:\FTP\Exemples de log\RCM.log.37176");

			Console.WriteLine(string.Join("\t", reader.FormatHandler.Rules[0].GetColumns()));

			while (true)
			{
				//log = reader.ReadLog();
				ev = reader.ReadEvent();
				if (ev.Rule==null)
				{
					//logger.Log(0, "Main", "Main", LogLevels.Warning, "Log doesn't match any rule");
					Console.ForegroundColor = ConsoleColor.Gray;
					Console.WriteLine(ev.Log);
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.White;
					Console.WriteLine(string.Join("\t",ev.Properties.Select(item=>item.Value)));
				}
				if (Console.ReadKey().Key == ConsoleKey.Escape) break;
			}

		}
	}
}
