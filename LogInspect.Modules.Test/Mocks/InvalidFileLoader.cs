using LogInspect.BaseLib.FileLoaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules.Test.Mocks
{
	public class InvalidFileLoader : IFileLoader<string>
	{
		public string Load(string FileName)
		{
			throw new Exception("Failed to load file");
		}

	}
}
