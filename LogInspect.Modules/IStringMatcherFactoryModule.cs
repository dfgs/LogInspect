using LogInspect.BaseLib;
using ModuleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public interface IStringMatcherFactoryModule:IModule
	{
		IStringMatcher CreateStringMatcher(string NameSpace, IEnumerable<string> Patterns);
	}
}
