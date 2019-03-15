using LogInspect.BaseLib.FileLoaders;
using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules.Test.Mocks
{
	public class MockedInlineFormatCollectionLoader : IFileLoader<InlineFormatCollection>
	{
		public InlineFormatCollection Load(string FileName)
		{
			InlineFormatCollection list= new InlineFormatCollection() { NameSpace=FileName} ;
			list.Items.Add(new InlineFormat() { Name = "Format1" });
			list.Items.Add(new InlineFormat() { Name = "Format2" });
			list.Items.Add(new InlineFormat() { Name = "Format3" });
			return list;
		}
	}
}
