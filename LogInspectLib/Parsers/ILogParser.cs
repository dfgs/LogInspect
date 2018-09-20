using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspectLib.Parsers
{
	public interface ILogParser:IParser
	{
		Event Parse(Log Log);
		void Add(IEnumerable<Regex> Regexes);
		void Add(Regex Regex);
		void Add(string Pattern);
	}
}
