using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspectLib
{
	public class RegexBuilder
	{
		private static Regex patternExtract=new Regex(@"{(?<Pattern>\w+)}|(?<Regex>.[^{]*)");

		private Dictionary<string,Pattern> patterns;
		public Dictionary<string, string> cache;


		public RegexBuilder()
		{
			patterns = new Dictionary<string, Pattern>();
			cache = new Dictionary<string, string>();
		}

		public void Add(Pattern Pattern)
		{
			cache.Clear();
			if (patterns.ContainsKey(Pattern.Name)) patterns.Remove(Pattern.Name);
			patterns.Add(Pattern.Name, Pattern);
		}

		public void Add(IEnumerable<Pattern> Patterns)
		{
			foreach (Pattern item in Patterns)
			{
				Add(item);
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
				if (!string.IsNullOrEmpty(group.Value)) regex = group.Value;
				else
				{
					subPatternName = match.Groups["Pattern"].Value;
					if (!cache.TryGetValue(subPatternName,out regex))
					{
						if (!patterns.TryGetValue(subPatternName, out subPattern)) throw new KeyNotFoundException($"Pattern {subPatternName} doesn't exist in regex builder");
						regex = BuildRegexPattern(subPattern.Value);
						cache.Add(subPatternName, regex);
					}
				}
				sb.Append(regex);
			}
			return sb.ToString();
		}


		public Regex Build(Pattern Pattern)
		{
			return new Regex(BuildRegexPattern(Pattern.Value), RegexOptions.Compiled);
		}
		public Regex Build(string Pattern)
		{
			return new Regex(BuildRegexPattern(Pattern),RegexOptions.Compiled);
		}

	}
}
