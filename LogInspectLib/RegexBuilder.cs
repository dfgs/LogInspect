using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspectLib
{
	public class RegexBuilder:NameSpaceDictionary<Pattern>, IRegexBuilder
	{
		private static Regex patternExtract=new Regex(@"{(?<Pattern>[\w\.]+)}|(?<Regex>.[^{]*)");

		public RegexBuilder()
		{
		}


		public void Add(string NameSpace, Pattern Pattern)
		{
			Add(NameSpace, Pattern.Name, Pattern);
		}
		public void Add(string NameSpace, IEnumerable<Pattern> Patterns)
		{
			foreach (Pattern item in Patterns)
			{
				Add(NameSpace, item.Name, item);
			}
		}

	

		public string BuildRegexPattern(string DefaultNameSpace,string Pattern)
		{
			MatchCollection matches;
			StringBuilder sb;
			Group group;
			string regex;
			Pattern subPattern;

			sb = new StringBuilder();
			matches = patternExtract.Matches(Pattern);
			foreach (Match match in matches)
			{
				group = match.Groups["Regex"];
				if (!string.IsNullOrEmpty(group.Value))
				{
					regex = group.Value;
					sb.Append(regex);
					continue;
				}

				group = match.Groups["Pattern"];
				if (!string.IsNullOrEmpty(group.Value))
				{
					subPattern = GetItem(DefaultNameSpace,group.Value);
					regex = BuildRegexPattern(DefaultNameSpace, subPattern.Value);
					
					sb.Append(regex);
					continue;
				}
			}
			return sb.ToString();
		}

		public Regex Build(string DefaultNameSpace, string Pattern)
		{
			return new Regex(BuildRegexPattern(DefaultNameSpace, Pattern),RegexOptions.Compiled, TimeSpan.FromSeconds(2));
		}

		

	}
}
