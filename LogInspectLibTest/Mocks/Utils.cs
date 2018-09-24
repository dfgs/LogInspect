﻿using LogInspectLib;
using LogInspectLib.Loaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLibTest.Mocks
{
	public static class Utils
	{
		public static IRegexBuilder EmptyRegexBuilder = new RegexBuilder();
		public static IStringMatcher EmptyStringMatcher = new StringMatcher();
		public static ILogLoader EmptyLogLoader = new LogLoader(new MockedLineReader(),Utils.EmptyStringMatcher,Utils.EmptyStringMatcher);
	}
}
