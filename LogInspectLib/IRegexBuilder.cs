﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspectLib
{
	public interface IRegexBuilder:INameSpaceDictionary<Pattern>
	{

		void Add(string NameSpace,Pattern Pattern);
		void Add(string NameSpace, IEnumerable<Pattern> Patterns);

		string BuildRegexPattern(string DefaultNameSpace, string Pattern);
		Regex Build(string DefaultNameSpace,string Pattern);


	}
}
