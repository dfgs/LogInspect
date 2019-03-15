using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.BaseLib.FileLoaders
{
	public class PatternLibLoader : IFileLoader<PatternLib>
	{
		public PatternLib Load(string FileName)
		{
			return PatternLib.LoadFromFile(FileName);
		}
	}
}
