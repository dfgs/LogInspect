using LogInspect.BaseLib;
using LogInspect.BaseLib.FileLoaders;
using LogInspect.BaseLib.Parsers;
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
			ILogger logger;
			IPatternLibraryModule patternLibraryModule;
			IInlineFormatLibraryModule inlineFormatLibraryModule;
			IFormatHandlerLibraryModule formatHandlerLibraryModule;
			LogDumperModule dumper;

			if (args.Length == 0) return;

			logger = new FileLogger(new DefaultLogFormatter(), "LogInspectCLI.log");

			patternLibraryModule = new PatternLibraryModule(logger, new DirectoryEnumerator(), new PatternLibLoader());
			inlineFormatLibraryModule = new InlineFormatLibraryModule(logger, new DirectoryEnumerator(), new InlineColoringRuleLibLoader());
			formatHandlerLibraryModule = new FormatHandlerLibraryModule(logger, new DirectoryEnumerator(), new FormatHandlerLoader(), patternLibraryModule);

			patternLibraryModule.LoadDirectory(Properties.Settings.Default.PatternsFolder);
			inlineFormatLibraryModule.LoadDirectory(Properties.Settings.Default.InlineFormatsFolder);
			formatHandlerLibraryModule.LoadDirectory(Properties.Settings.Default.FormatHandlersFolder);


			dumper = new LogDumperModule(logger, args[0],formatHandlerLibraryModule.GetFormatHandler(args[0]),patternLibraryModule);
			dumper.Dump(100);

			Console.ReadLine();

		}

	}
}
