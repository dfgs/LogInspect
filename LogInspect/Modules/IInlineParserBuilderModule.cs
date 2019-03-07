using LogInspectLib;
using LogInspectLib.Parsers;
using ModuleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public interface IInlineParserBuilderModule:IModule
	{
		IInlineParser CreateParser(string NameSpace,Column Column);
	}
}
