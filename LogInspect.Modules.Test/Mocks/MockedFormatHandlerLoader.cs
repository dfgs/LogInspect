using LogInspect.BaseLib.FileLoaders;
using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules.Test.Mocks
{
	public class MockedFormatHandlerLoader : IFileLoader<FormatHandler>
	{
		public FormatHandler Load(string FileName)
		{
			return new FormatHandler() {  Name=FileName, FileNamePattern=FileName};
		}
	}
}
