using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LogInspect.BaseLib;
using LogInspect.BaseLib.FileLoaders;
using LogInspect.Models;
using LogLib;

namespace LogInspect.Modules
{
	public class PatternLibraryModule : LibraryModule<PatternCollection>, IPatternLibraryModule
	{
		private RegexBuilder regexBuilder;
		
		public int Count
		{
			get { return regexBuilder.Count; }
		}
		

		public PatternLibraryModule(ILogger Logger, IDirectoryEnumerator DirectoryEnumerator, IFileLoader<PatternCollection> FileLoader) : base(Logger,DirectoryEnumerator,FileLoader)
		{
			regexBuilder = new RegexBuilder();
		}

		protected override void OnItemLoaded(PatternCollection Item)
		{
			foreach(Pattern pattern in  Item.Items)
			{
				regexBuilder.Add(Item.NameSpace, pattern);
			}
		}

		public Regex Build(string DefaultNameSpace, string Pattern, bool IgnoreCase)
		{
			Regex regex;
			if (!Try(() => regexBuilder.Build(DefaultNameSpace, Pattern, IgnoreCase)).OrAlert(out regex, (ex) => ex.Message)) return null;
			return regex;
		}

		public string BuildRegexPattern(string DefaultNameSpace, string Pattern)
		{
			string regex;
			if (!Try(() => regexBuilder.BuildRegexPattern(DefaultNameSpace, Pattern)).OrAlert(out regex, (ex) => ex.Message)) return null;
			return regex;
		}

		public Pattern GetItem(string Name)
		{
			Pattern pattern;
			if (!Try(() => regexBuilder.GetItem(Name)).OrAlert(out pattern, (ex) => ex.Message)) return null;
			return pattern;
		}

		public Pattern GetItem(string DefaultNameSpace, string Name)
		{
			Pattern pattern;
			if (!Try(() => regexBuilder.GetItem(DefaultNameSpace,Name)).OrAlert(out pattern, (ex) => ex.Message)) return null;
			return pattern;
		}

	




	}
}
