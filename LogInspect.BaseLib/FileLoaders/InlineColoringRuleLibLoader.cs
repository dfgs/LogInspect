using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.BaseLib.FileLoaders
{
	public class InlineColoringRuleLibLoader : IFileLoader<InlineFormatCollection>
	{
		public InlineFormatCollection Load(string FileName)
		{
			return InlineFormatCollection.LoadFromFile(FileName);
		}
	}
}
