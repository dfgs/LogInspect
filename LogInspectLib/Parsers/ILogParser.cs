using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLib.Parsers
{
	public interface ILogParser:IParser
	{
		Event Parse(Log Log);
		void Add(string DefaultNameSpace, string Pattern);
	}
}
