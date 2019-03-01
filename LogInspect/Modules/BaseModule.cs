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

		public int MaxRate
		{
			get { return rateMeter.MaxRate; }
		}

		public abstract int Count
		{
			get;
		}
		public abstract int ProceededCount
		{
			get;
		}
		public AutoResetEvent ProceededEvent
		{
			get;
			private set;
		}

		private int lookupRetryDelay;
		private DateTime startTime;
		private RateMeter rateMeter;
		private WaitHandle[] events;

		public BaseModule(ILogger Logger, int LookupRetryDelay,WaitHandle LookUpRetryEvent,ThreadPriority Priority) : base(Logger,Priority)
		{
			this.lookupRetryDelay = LookupRetryDelay;
			rateMeter = new RateMeter(1);
			if (LookUpRetryEvent == null) events = new WaitHandle[] { QuitEvent };
			else events = new WaitHandle[] { QuitEvent,LookUpRetryEvent };
			ProceededEvent = new AutoResetEvent(false);
		}

		protected abstract int OnProcess();

		protected override sealed void ThreadLoop()
		{
			int result;

			startTime = DateTime.Now;rateMeter.Start();
			while(State == ModuleStates.Started)
			{
				while((State == ModuleStates.Started) && (CanProcess))
				{
					result=OnProcess();
					if (result<=0) break;
					ProceededEvent.Set();
					rateMeter.Refresh(result);
				}

				rateMeter.Refresh(0);
				Log(LogLevels.Debug, $"Module reached target in {DateTime.Now - startTime}");
				startTime = DateTime.Now;
				WaitHandles(lookupRetryDelay, events);
			}

		}
		
		


	}
}
