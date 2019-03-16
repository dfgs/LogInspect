using LogInspect.BaseLib.FileLoaders;
using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules.Test.Mocks
{
	public class MockedPatternCollectionLoader : IFileLoader<PatternCollection>
	{
		public PatternCollection Load(string FileName)
		{
			PatternCollection list = new PatternCollection() { NameSpace = FileName };
			list.Items.Add(new Pattern() { Name = "Format1" });
			list.Items.Add(new Pattern() { Name = "Format2" });
			list.Items.Add(new Pattern() { Name = "Format3" });
			return list;
		}
	}
}
