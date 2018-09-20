using LogInspectLib;
using LogInspectLib.Loaders;
using LogLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public abstract class LoaderModule<TLoader,T> : BaseModule,ILoaderModule<TLoader, T>
		where TLoader:ILoader<T>
	{
		public override bool CanProcess
		{
			get { return Loader.CanLoad; }
		}

		public TLoader Loader
		{
			get;
			private set;
		}

		

		public LoaderModule(string Name,ILogger Logger,TLoader Loader,WaitHandle LookUpRetryEvent, int LookupRetryDelay) :base(Name,Logger,LookupRetryDelay, LookUpRetryEvent, System.Threading.ThreadPriority.Lowest)
		{
			this.Loader = Loader;
		}

		protected override bool OnProcessItem()
		{

			try
			{
				Loader.Load();
				return true;
			}
			catch(EndOfStreamException)
			{
				Log(LogLevels.Information, "Module reached end of items");
				return false;
			}
			catch(Exception ex)
			{
				Log(ex);
				return false;
			}
			
		}
		

	}
}
