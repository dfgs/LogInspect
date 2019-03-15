using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.BaseLib.FileLoaders
{
	public class InlineColoringRuleLibLoader : IFileLoader<InlineColoringRuleLib>
	{
		public InlineColoringRuleLib Load(string FileName)
		{
			return InlineColoringRuleLib.LoadFromFile(FileName);
		}
	}
}
