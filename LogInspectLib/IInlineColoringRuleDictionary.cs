using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLib
{
	public interface IInlineColoringRuleDictionary:INameSpaceDictionary<InlineColoringRule>
	{
		void Add(string NameSpace, InlineColoringRule InlineColoringRule);
		void Add(string NameSpace, IEnumerable<InlineColoringRule> InlineColoringRules);


	}
}
