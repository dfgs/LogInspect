using LogInspect.BaseLib.FileLoaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules.Test.Mocks
{
	public class MockedFileLoader : IFileLoader<string>
	{
		public string Load(string FileName)
		{
			return FileName;
		}
	}
}
