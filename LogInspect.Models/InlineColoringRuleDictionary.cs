using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Models
{
	public class InlineColoringRuleDictionary:NameSpaceDictionary<InlineColoringRule>,IInlineColoringRuleDictionary
	{


		public void Add(string NameSpace, InlineColoringRule InlineColoringRule)
		{
			Add(NameSpace, InlineColoringRule.Name, InlineColoringRule);
		}
		public void Add(string NameSpace, IEnumerable<InlineColoringRule> InlineColoringRules)
		{
			foreach (InlineColoringRule item in InlineColoringRules)
			{
				Add(NameSpace, item.Name, item);
			}
		}

	}
}
