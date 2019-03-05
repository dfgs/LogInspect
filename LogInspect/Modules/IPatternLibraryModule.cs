using LogInspectLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public interface IPatternLibraryModule:ILibraryModule<FormatHandler>
	{
		IRegexBuilder RegexBuilder
		{
			get;
		}

	}
}
