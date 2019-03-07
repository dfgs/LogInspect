﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogInspectLib;
using LogInspectLib.Parsers;
using LogLib;
using ModuleLib;

namespace LogInspect.Modules
{
	public class InlineParserBuilderModule : Module, IInlineParserBuilderModule
	{
		private IRegexBuilder regexBuilder;
		private IInlineColoringRuleDictionary inlineColoringRuleDictionary;

		public InlineParserBuilderModule(ILogger Logger, IRegexBuilder RegexBuilder, IInlineColoringRuleDictionary InlineColoringRuleDictionary) : base(Logger)
		{
			AssertParameterNotNull("RegexBuilder", RegexBuilder);
			AssertParameterNotNull("InlineColoringRuleDictionary", InlineColoringRuleDictionary);

			this.regexBuilder = RegexBuilder;
			this.inlineColoringRuleDictionary = InlineColoringRuleDictionary;
		}

		public IInlineParser CreateParser(string NameSpace, Column Column)
		{
			IInlineParser inlineParser;
			InlineColoringRule inlineColoringRule;

			inlineParser = new InlineParser(regexBuilder);
			foreach (string ruleName in Column.InlineColoringRules)
			{
				try
				{
					inlineColoringRule = inlineColoringRuleDictionary.GetItem(NameSpace, ruleName);
				}
				catch (Exception ex)
				{
					Log(LogLevels.Warning, ex.Message);
					continue;
				}
				try
				{
					inlineParser.Add(NameSpace, inlineColoringRule);
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
