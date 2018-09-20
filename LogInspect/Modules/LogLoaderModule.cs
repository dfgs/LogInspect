using LogInspectLib;
using LogInspectLib.Loaders;
using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public class LogLoaderModule : LoaderModule<ILogLoader, Log>, ILogLoaderModule
	{
		public LogLoaderModule( ILogger Logger, ILogLoader Loader, WaitHandle LookUpRetryEvent, int LookupRetryDelay) : base("LogLoader", Logger, Loader,LookUpRetryEvent, LookupRetryDelay)
		{
		}

	}
}
