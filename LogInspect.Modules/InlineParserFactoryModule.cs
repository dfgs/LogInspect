using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogInspect.BaseLib;
using LogInspect.BaseLib.Parsers;
using LogInspect.Models;
using LogLib;
using ModuleLib;

namespace LogInspect.Modules
{
	public class InlineParserFactoryModule : Module, IInlineParserFactoryModule
	{
		private IRegexBuilder regexBuilder;
		private IInlineFormatLibraryModule inlineFormatLibraryModule;

		public InlineParserFactoryModule(ILogger Logger, IRegexBuilder RegexBuilder, IInlineFormatLibraryModule InlineFormatLibraryModule) : base(Logger)
		{
			AssertParameterNotNull(RegexBuilder,"RegexBuilder", out regexBuilder );
			AssertParameterNotNull(InlineFormatLibraryModule, "InlineFormatLibraryModule", out inlineFormatLibraryModule);

		}

		public IInlineParser CreateParser(string DefaultNameSpace, Column Column)
		{
			InlineParser inlineParser;
			InlineFormat inlineColoringRule;

			if (!AssertParameterNotNull(DefaultNameSpace, "DefaultNameSpace")) return null;
			if (!AssertParameterNotNull(Column, "Column")) return null;

			inlineParser = new InlineParser(regexBuilder);
			foreach (string ruleName in Column.InlineColoringRules)
			{
				try
				{
					inlineColoringRule = inlineFormatLibraryModule.GetItem(DefaultNameSpace, ruleName);
				}
				catch (Exception ex)
				{
					Log(LogLevels.Warning, ex.Message);
					continue;
				}
				try
				{
					inlineParser.Add(DefaultNameSpace, inlineColoringRule);
				}
				catch (Exception ex)
				{
					Log(LogLevels.Warning, ex.Message);
					continue;
				}
			}

			return inlineParser;
		}


	}
}
