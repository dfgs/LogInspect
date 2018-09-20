using LogInspect.Models;
using LogInspectLib;
using LogInspectLib.Parsers;
using LogLib;
using ModuleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public abstract class BaseModule: ThreadModule,IBaseModule
	{


		public abstract bool CanProcess
		{
			get;
		}
		
		public int Rate
		{
			get { return rateMeter.Rate; }
		}
		public int Count
		{
			get;
			private set;
		}
		
		private int lookupRetryDelay;
		private DateTime startTime;
		private RateMeter rateMeter;
	
		public BaseModule(string Name, ILogger Logger, int LookupRetryDelay,ThreadPriority Priority) : base(Name, Logger,Priority)
		{
			this.lookupRetryDelay = LookupRetryDelay;
			rateMeter = new RateMeter(1);
		}

		protected abstract bool OnProcessItem();

		protected override sealed void ThreadLoop()
		{
			bool result;

			startTime = DateTime.Now;rateMeter.Start();
			while(State == ModuleStates.Started)
			{
				while((State == ModuleStates.Started) && (CanProcess))
				{
					result=OnProcessItem();
					if (!result) break;
					Count++;
					rateMeter.Refresh(1);
				}

				rateMeter.Refresh(0);
				Log(LogLevels.Debug, $"Module reached target in {DateTime.Now - startTime}");
				startTime = DateTime.Now;
				WaitHandles(lookupRetryDelay, QuitEvent);
			}

		}
		
		


	}
}
