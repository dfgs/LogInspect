using LogInspect.BaseLib;
using LogInspect.BaseLib.Builders;
using LogInspect.BaseLib.FileLoaders;
using LogInspect.BaseLib.Parsers;
using LogInspect.Models;
using LogInspect.Modules;
using LogLib;
using System;
using System.Collections.Generic;
using System.IO;
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
			IStringMatcherFactoryModule stringMatcherFactoryModule;
			LogFileLoaderModule dumper;
			ILineReader lineReader;
			ILineBuilder lineBuilder;
			ILogBuilder logBuilder;
			LogParser logParser;
			IEventList eventList;

			FormatHandler formatHandler;

			if (args.Length == 0) return;

			logger = new FileLogger(new DefaultLogFormatter(), "LogInspectCLI.log");

			patternLibraryModule = new PatternLibraryModule(logger, new DirectoryEnumerator(), new PatternLibLoader());
			inlineFormatLibraryModule = new InlineFormatLibraryModule(logger, new DirectoryEnumerator(), new InlineColoringRuleLibLoader());
			formatHandlerLibraryModule = new FormatHandlerLibraryModule(logger, new DirectoryEnumerator(), new FormatHandlerLoader(), patternLibraryModule);

			patternLibraryModule.LoadDirectory(Properties.Settings.Default.PatternsFolder);
			inlineFormatLibraryModule.LoadDirectory(Properties.Settings.Default.InlineFormatsFolder);
			formatHandlerLibraryModule.LoadDirectory(Properties.Settings.Default.FormatHandlersFolder);

			stringMatcherFactoryModule = new StringMatcherFactoryModule(logger,patternLibraryModule);
			eventList = new ConsoleEventList();
			formatHandler = formatHandlerLibraryModule.GetFormatHandler(args[0]);


			lineBuilder = new LineBuilder(stringMatcherFactoryModule.CreateStringMatcher(formatHandler.NameSpace, formatHandler.DiscardLinePatterns));
			logBuilder = new LogBuilder(
				stringMatcherFactoryModule.CreateStringMatcher(formatHandler.NameSpace, formatHandler.Rules.Where(item => item.Discard).Select(item => item.GetPattern())),
				stringMatcherFactoryModule.CreateStringMatcher(formatHandler.NameSpace, formatHandler.AppendLineToPreviousPatterns),
				stringMatcherFactoryModule.CreateStringMatcher(formatHandler.NameSpace, formatHandler.AppendLineToNextPatterns)
				);
			logParser = new LogParser(formatHandler.Columns);
			foreach (Rule rule in formatHandler.Rules)
			{
				logParser.Add(patternLibraryModule.Build(formatHandler.NameSpace, rule.GetPattern(),true));
			}

			using (FileStream stream = new FileStream(args[0], FileMode.Open))
			{
				lineReader = new FileLineReader(stream);
				dumper = new LogFileLoaderModule(logger,lineReader,lineBuilder,logBuilder,logParser,eventList);
				dumper.Load();
			}

			Console.ReadLine();

		}

	}
}
