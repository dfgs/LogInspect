﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogInspect.Models;
using LogLib;

namespace LogInspect.Modules
{
	public class InlineColoringRuleLibraryModule : LibraryModule<InlineColoringRuleLib>, IInlineColoringRuleLibraryModule
	{
		private IInlineColoringRuleDictionary inlineColoringRuleDictionary;
		public IInlineColoringRuleDictionary InlineColoringRuleDictionary
		{
			get { return inlineColoringRuleDictionary; }
		}

		public InlineColoringRuleLibraryModule(ILogger Logger, IInlineColoringRuleDictionary InlineColoringRuleDictionary) : base(Logger)
		{
			AssertParameterNotNull(InlineColoringRuleDictionary,"InlineColoringRuleDictionary", out inlineColoringRuleDictionary);
		}

		protected override InlineColoringRuleLib OnLoadFile(string FileName)
		{
			return InlineColoringRuleLib.LoadFromFile(FileName);
		}
		protected override void OnItemLoaded(InlineColoringRuleLib Item)
		{
			InlineColoringRuleDictionary.Add(Item.NameSpace, Item.Items);
		}

		
	}
}
