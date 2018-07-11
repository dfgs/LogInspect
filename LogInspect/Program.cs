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

			rule = new LogInspectLib.Rule() { Name = "Event" };
			rule.Tokens.Add(new Token() { Name = "Date", Pattern = @"^\d\d/\d\d/\d\d \d\d:\d\d:\d\d\.\d+" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @" *\| *" });
			rule.Tokens.Add(new Token() { Name = "Severity", Pattern = @"[^ ]+" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @" *\| *" });
			rule.Tokens.Add(new Token() { Name = "Class", Pattern = @"[^,]+" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @", " });
			rule.Tokens.Add(new Token() { Name = "Thread", Pattern = @"\d+" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @" *\| *" });
			rule.Tokens.Add(new Token() { Name = "Module", Pattern = @"[^ ]+" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @" *\| *" });
			rule.Tokens.Add(new Token() { Name = "Message", Pattern = @"[^\u0003]+" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @"\u0003$" });
			schema.Rules.Add(rule);

			rule = new LogInspectLib.Rule() { Name = "Comment" };
			rule.Tokens.Add(new Token() { Pattern = @"^//" });
			schema.Rules.Add(rule);


			schema.SaveToFile(@"FormatHandlers\RCM.xml");

			schemaManager = new LogFileManager(logger);
			schemaManager.LoadSchemas("FormatHandlers");

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
