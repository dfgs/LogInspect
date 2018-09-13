﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspectLib
{
	public interface IRegexBuilder
	{

		void Add(string NameSpace,Pattern Pattern);
		void Add(string NameSpace, IEnumerable<Pattern> Patterns);

		string BuildRegexPattern(string Pattern);
		//Regex Build(Pattern Pattern);
		Regex Build(string Pattern);


	}
}
