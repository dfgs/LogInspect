using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspectLib
{
	public interface IStringMatcher
	{
		bool Match(string Value);
		Match GetMatch(string Value);
		void Add( IEnumerable<Regex> Regexes);
		void Add( Regex Regex);
		void Add(string Pattern);
	}
}
