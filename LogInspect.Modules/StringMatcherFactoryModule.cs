using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LogInspect.BaseLib;
using LogLib;
using ModuleLib;

namespace LogInspect.Modules
{
	public class StringMatcherFactoryModule : Module, IStringMatcherFactoryModule
	{
		private IRegexBuilder regexBuilder;

		public StringMatcherFactoryModule(ILogger Logger, IRegexBuilder RegexBuilder) : base(Logger)
		{
			AssertParameterNotNull(RegexBuilder, "RegexBuilder", out regexBuilder);
		}

		public IStringMatcher CreateStringMatcher(string NameSpace, IEnumerable<string> Patterns)
		{
			StringMatcher matcher;
			Regex regex;

			matcher = new StringMatcher();
			foreach (string pattern in Patterns)
			{
				if (!Try(() => regexBuilder.Build(NameSpace, pattern, false)).OrAlert(out regex, $"Failed to build regex: {pattern}")) continue;
				matcher.Add(regex);
			}
			return matcher;
		}


	}
}
