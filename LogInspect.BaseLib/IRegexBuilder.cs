using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspect.BaseLib
{
	public interface IRegexBuilder:INameSpaceDictionary<Pattern>
	{


		string BuildRegexPattern(string DefaultNameSpace, string Pattern);
		Regex Build(string DefaultNameSpace,string Pattern,bool IgnoreCase);


	}
}
