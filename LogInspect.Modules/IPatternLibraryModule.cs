using LogInspect.BaseLib;
using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public interface IPatternLibraryModule:ILibraryModule<FormatHandler>,IRegexBuilder
	{
		

	}
}
