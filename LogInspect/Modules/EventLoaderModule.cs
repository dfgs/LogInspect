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
	public class EventLoaderModule : LoaderModule<IEventLoader, Event>, IEventLoaderModule
	{
		public EventLoaderModule( ILogger Logger, IEventLoader Loader, WaitHandle LookUpRetryEvent, int LookupRetryDelay) : base("EventLoader", Logger, Loader, LookUpRetryEvent, LookupRetryDelay)
		{
		}

	}
}
