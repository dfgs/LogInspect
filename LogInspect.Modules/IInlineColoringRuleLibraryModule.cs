using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public interface IInlineColoringRuleLibraryModule : ILibraryModule<InlineColoringRule>
	{
		InlineColoringRuleDictionary InlineColoringRuleDictionary
		{
			get;
		}

	}
}
