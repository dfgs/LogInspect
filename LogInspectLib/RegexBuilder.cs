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
		private static Regex patternExtract=new Regex(@"{(?<NameSpacePattern>\w+(\.\w+)+)}|{(?<Pattern>\w+)}|(?<Regex>.[^{]*)");

		private Dictionary<string, Pattern> patterns;
		private Dictionary<string, Pattern> fullNamedPatterns;
		public Dictionary<string, string> cache;


		public RegexBuilder()
		{
			fullNamedPatterns = new Dictionary<string, Pattern>();
			patterns = new Dictionary<string, Pattern>();
			cache = new Dictionary<string, string>();
		}

		private string GetFullPatterName(string NameSpace,string PatternName)
		{
			return $"{NameSpace}.{PatternName}";
		}

		public void Add(string NameSpace,Pattern Pattern)
		{
			string fullName;

			fullName = GetFullPatterName(NameSpace, Pattern.Name);
			cache.Clear();
			if (fullNamedPatterns.ContainsKey(fullName)) fullNamedPatterns.Remove(fullName);
			fullNamedPatterns.Add(fullName, Pattern);
			if (patterns.ContainsKey(Pattern.Name)) patterns.Remove(Pattern.Name);
			patterns.Add(Pattern.Name, Pattern);
		}

		public void Add(string NameSpace, IEnumerable<Pattern> Patterns)
		{
			foreach (Pattern item in Patterns)
			{
				Add(NameSpace,item);
			}
		}
		public string BuildRegexPattern(string DefaultNameSpace,string Pattern)
		{
			MatchCollection matches;
			StringBuilder sb;
			Group group;
			string regex;
			string fullName;
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
				group = match.Groups["NameSpacePattern"];
				if (!string.IsNullOrEmpty(group.Value))
				{
					fullName = group.Value;
					if (!cache.TryGetValue(fullName, out regex))
					{
						if  (!fullNamedPatterns.TryGetValue(fullName, out subPattern)) throw new KeyNotFoundException($"Pattern {fullName} doesn't exist in regex builder");
						regex = BuildRegexPattern(DefaultNameSpace, subPattern.Value);
						cache.Add(fullName, regex);
					}
					sb.Append(regex);
					continue;
				}
				group = match.Groups["Pattern"];
				if (!string.IsNullOrEmpty(group.Value))
				{
					fullName = GetFullPatterName(DefaultNameSpace,group.Value);
					if (!cache.TryGetValue(fullName, out regex))
					{
						if ( (!fullNamedPatterns.TryGetValue(fullName, out subPattern))  && (!patterns.TryGetValue(group.Value, out subPattern))) throw new KeyNotFoundException($"Pattern {fullName} doesn't exist in regex builder");
						regex = BuildRegexPattern(DefaultNameSpace, subPattern.Value);
						cache.Add(fullName, regex);
					}
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

		public void ClearCache()
		{
			cache.Clear();
		}
	}
}
