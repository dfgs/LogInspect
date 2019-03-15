using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspect.BaseLib
{
	public interface IStringMatcher
	{
		bool Match(string Value);
		Match GetMatch(string Value);
		
	}
}
