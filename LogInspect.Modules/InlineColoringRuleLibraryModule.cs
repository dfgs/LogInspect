using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogInspect.BaseLib;
using LogInspect.BaseLib.FileLoaders;
using LogInspect.Models;
using LogLib;

namespace LogInspect.Modules
{
	public class InlineColoringRuleLibraryModule : LibraryModule<InlineColoringRuleLib>, IInlineColoringRuleLibraryModule
	{
		private InlineColoringRuleDictionary inlineColoringRuleDictionary;
		public InlineColoringRuleDictionary InlineColoringRuleDictionary
		{
			get { return inlineColoringRuleDictionary; }
		}

		public InlineColoringRuleLibraryModule(ILogger Logger, IDirectoryEnumerator DirectoryEnumerator, IFileLoader<InlineColoringRuleLib> FileLoader, InlineColoringRuleDictionary InlineColoringRuleDictionary) : base(Logger,DirectoryEnumerator,FileLoader)
		{
			AssertParameterNotNull(InlineColoringRuleDictionary,"InlineColoringRuleDictionary", out inlineColoringRuleDictionary);
		}

		
		protected override void OnItemLoaded(InlineColoringRuleLib Item)
		{
			InlineColoringRuleDictionary.Add(Item.NameSpace, Item.Items);
		}

		
	}
}
