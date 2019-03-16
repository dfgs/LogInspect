using LogInspect.BaseLib.Parsers;
using LogInspect.Models;
using ModuleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public interface IInlineParserFactoryModule:IModule
	{
		IInlineParser CreateParser(string DefaultNameSpace,Column Column);
	}
}
