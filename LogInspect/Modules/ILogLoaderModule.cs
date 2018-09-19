using LogInspectLib;
using LogInspectLib.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public interface ILogLoaderModule:ILoaderModule<ILogLoader,Log>
	{
	}
}
