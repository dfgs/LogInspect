using LogInspect.BaseLib;
using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.BaseLib.Test.Mocks
{
	public static class Utils
	{
		public static IRegexBuilder EmptyRegexBuilder = new RegexBuilder();
		public static IStringMatcher EmptyStringMatcher = new StringMatcher();
	}
}
