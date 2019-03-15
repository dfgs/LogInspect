using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspect.BaseLib.Parsers
{
	public interface ILogParser:IParser
	{
		Event Parse(Log Log);
		/*void Add(Regex Regex,bool Discard);
		void Add(string Pattern,bool Discard);*/
	}
}
