using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogInspect.Models;
using LogLib;

namespace LogInspect.Modules
{
	public class PatternLibraryModule : LibraryModule<PatternLib>, IPatternLibraryModule
	{
		public IRegexBuilder RegexBuilder
		{
			get;
			private set;
		}

		public PatternLibraryModule(ILogger Logger,IRegexBuilder RegexBuilder) : base(Logger)
		{
			AssertParameterNotNull("RegexBuilder", RegexBuilder);
			this.RegexBuilder = RegexBuilder;
		}

		protected override PatternLib OnLoadFile(string FileName)
		{
			return PatternLib.LoadFromFile(FileName);
		}
		protected override void OnItemLoaded(PatternLib Item)
		{
			RegexBuilder.Add(Item.NameSpace, Item.Items);
		}

		
	}
}
