using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspectLib
{
	public class StringMatcher : IStringMatcher
	{
		private List<Regex> items;

		public StringMatcher()
		{
			items = new List<Regex>();
		}

		public void Add( Regex Regex)
		{
			items.Add(Regex);
		}
		public void Add(string Pattern)
		{
			items.Add(new Regex(Pattern,RegexOptions.Compiled));
		}
		public void Add( IEnumerable<Regex> Regexes)
		{
			items.AddRange(Regexes);
		}

		public bool Match(string Value)
		{
			foreach (Regex item in items)
			{
				if (item.Match(Value).Success) return true;
			}
			return false;
		}
		public Match GetMatch(string Value)
		{
			Match match;

			foreach (Regex item in items)
			{
				match = item.Match(Value);
				if (match.Success) return match;
			}
			return null;
		}
	}
}
