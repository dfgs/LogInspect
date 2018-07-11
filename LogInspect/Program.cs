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

			FormatHandler schema = new FormatHandler();
			schema.Name = "RCM";
			schema.FileNamePattern = @"^RCM\.log(\.\d+)?$";
			schema.AppendToNextPatterns.Add(@".*(?<!\u0003)$");

			rule = new LogInspectLib.Rule() { Name="Comment" };
			rule.Tokens.Add(new Token() { Pattern=@"^//" });
			schema.Rules.Add(rule);

			rule = new LogInspectLib.Rule() { Name = "Event" };
			rule.Tokens.Add(new Token() { Pattern = @"^\d\d/\d\d/\d\d" });
			schema.Rules.Add(rule);


			schema.SaveToFile(@"FormatHandlers\RCM.xml");

			schemaManager = new LogFileManager(logger);
			schemaManager.LoadSchemas("FormatHandlers");

			LogReader reader = schemaManager.CreateLogReader(@"E:\FTP\Exemples de log\RCM.log.37176");
			while(true)
			{
				//log = reader.ReadLog();
				ev = reader.ReadEvent();
				if (ev.Rule==null)
				{
					logger.Log(0, "Main", "Main", LogLevels.Warning, "Log doesn't match any rule");
				}
				Console.WriteLine(ev.Log);
				if (Console.ReadKey().Key == ConsoleKey.Escape) break;
			}

		}
	}
}
