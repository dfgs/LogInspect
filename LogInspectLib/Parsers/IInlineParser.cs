using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLib.Parsers
{
	public interface IInlineParser:IParser
	{
		IEnumerable<Inline> Parse(string Value);
		void Add(string DefaultNameSpace, string PatternName);

	}
}
