using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspectLib
{
	public class RegexBuilder:IRegexBuilder
	{
		private static Regex patternExtract=new Regex(@"{(?<Pattern>\w+(\.\w+)*)}|(?<Regex>.[^{]*)");

		private Dictionary<string, Pattern> patterns;
		private Dictionary<string, Pattern> patternsWithNameSpace;
		public Dictionary<string, string> cache;


		public RegexBuilder()
		{
			patterns = new Dictionary<string, Pattern>();
			patternsWithNameSpace = new Dictionary<string, Pattern>();
			cache = new Dictionary<string, string>();
		}

		private string FullPatterName(string NameSpace,Pattern Pattern)
		{
			return $"{NameSpace}.{Pattern.Name}";
		}

		public void Add(string NameSpace,Pattern Pattern)
		{
			string fullName;


			fullName = FullPatterName(NameSpace, Pattern);
			cache.Clear();
			if (patterns.ContainsKey(Pattern.Name)) patterns.Remove(Pattern.Name);
			patterns.Add(Pattern.Name, Pattern);
			if (patternsWithNameSpace.ContainsKey(fullName)) patternsWithNameSpace.Remove(fullName);
			patternsWithNameSpace.Add(fullName, Pattern);

		}

		public void Add(string NameSpace, IEnumerable<Pattern> Patterns)
		{
			foreach (Pattern item in Patterns)
			{
				Add(NameSpace,item);
			}
		}
		public string BuildRegexPattern(string Pattern)
		{
			MatchCollection matches;
			StringBuilder sb;
			Group group;
			string regex;
			string subPatternName;
			Pattern subPattern;

			sb = new StringBuilder();
			matches = patternExtract.Matches(Pattern);
			foreach (Match match in matches)
			{
				group = match.Groups["Regex"];
				if (!string.IsNullOrEmpty(group.Value))
				{
					regex = group.Value;
				}
				else
				{
					subPatternName = match.Groups["Pattern"].Value;
					if (!cache.TryGetValue(subPatternName, out regex))
					{
						if ( (!patternsWithNameSpace.TryGetValue(subPatternName, out subPattern)) && (!patterns.TryGetValue(subPatternName, out subPattern))) throw new KeyNotFoundException($"Pattern {subPatternName} doesn't exist in regex builder");
						regex = BuildRegexPattern(subPattern.Value);
						cache.Add(subPatternName, regex);
					}
				}
				sb.Append(regex);
			}
			return sb.ToString();
		}


		/*public Regex Build(Pattern Pattern)
		{
			return new Regex(BuildRegexPattern(Pattern.Value), RegexOptions.Compiled,TimeSpan.FromSeconds(2));
		}*/

		public Regex Build(string Pattern)
		{
			return new Regex(BuildRegexPattern(Pattern),RegexOptions.Compiled, TimeSpan.FromSeconds(2));
		}

	}
}
