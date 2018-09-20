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
	public class LineLoaderModule : LoaderModule<ILineLoader, Line>, ILineLoaderModule
	{
		
		public LineLoaderModule( ILogger Logger, ILineLoader Loader, WaitHandle LookUpRetryEvent, int LookupRetryDelay) : base("LineLoader", Logger, Loader,LookUpRetryEvent, LookupRetryDelay)
		{
		}

	}
}
