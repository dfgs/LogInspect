using LogInspect.BaseLib;
using LogInspect.BaseLib.FileLoaders;
using LogInspect.Models;
using LogInspect.Modules;
using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectCLI
{
	class Program
	{

		static void Main(string[] args)
		{
			if (args.Length == 0) return;

			string line;

			ILogger logger;
			IPatternLibraryModule patternLibraryModule;
			IInlineFormatLibraryModule inlineColoringRuleLibraryModule;
			IFormatHandlerLibraryModule formatHandlerLibraryModule;
			LogFile logFile;

			logger = new FileLogger(new DefaultLogFormatter(),"LogInspectCLI.log");

			patternLibraryModule = new PatternLibraryModule(logger, new DirectoryEnumerator(), new PatternLibLoader(), new RegexBuilder());
			inlineColoringRuleLibraryModule = new InlineFormatLibraryModule(logger, new DirectoryEnumerator(), new InlineColoringRuleLibLoader(), new InlineColoringRuleDictionary());
			formatHandlerLibraryModule = new FormatHandlerLibraryModule(logger, new DirectoryEnumerator(), new FormatHandlerLoader(), patternLibraryModule.RegexBuilder);

			patternLibraryModule.LoadDirectory(Properties.Settings.Default.PatternLibsFolder);
			inlineColoringRuleLibraryModule.LoadDirectory(Properties.Settings.Default.InlineFormatsFolder);
			formatHandlerLibraryModule.LoadDirectory(Properties.Settings.Default.FormatHandlersFolder);


			logFile = new LogFile(args[0], formatHandlerLibraryModule.GetFormatHandler(args[0]));

			LogFileLoaderModule loader = new LogFileLoaderModule(logger, patternLibraryModule.RegexBuilder);
			loader.Load(logFile);

			foreach(Event ev in logFile.Events.Take(100))
			{
				line = string.Join("	", ev.Properties);
				if (line.Length > Console.BufferWidth)
				{
					line = line.Substring(0, Console.BufferWidth - 3) + "...";
				}
				Console.WriteLine(line);
				
			}
			

			Console.ReadLine();

		}

	}
}
