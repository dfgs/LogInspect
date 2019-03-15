using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.BaseLib.FileLoaders
{
	public class FormatHandlerLoader : IFileLoader<FormatHandler>
	{
		public FormatHandler Load(string FileName)
		{
			return FormatHandler.LoadFromFile(FileName);
		}
	}
}
