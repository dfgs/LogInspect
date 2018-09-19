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
	public abstract class BaseModule: ThreadModule
	{

		/*public abstract long Position
		{
			get;
		}
		public abstract long Target
		{
			get;
		}*/

		public abstract bool CanProcess
		{
			get;
		}
		
		public int Rate
		{
			get;
			private set;
		}

		private int lookupRetryDelay;
		private DateTime startTime;

	
		public BaseModule(string Name, ILogger Logger, int LookupRetryDelay,ThreadPriority Priority) : base(Name, Logger,Priority)
		{
			this.lookupRetryDelay = LookupRetryDelay;
		}

		protected abstract bool OnProcessItem();

		protected override sealed void ThreadLoop()
		{
			long startTick,tick;
			long count;
			double seconds;
			bool result;

			count = 0;
			startTick = Environment.TickCount;
			startTime = DateTime.Now;
			while(State == ModuleStates.Started)
			{
				while((State == ModuleStates.Started) && (CanProcess))
				{
					result=OnProcessItem();
					if (!result) break;
					count++;

					tick = Environment.TickCount;
					seconds = TimeSpan.FromTicks(tick - startTick).TotalSeconds;
					if (seconds>=1)
					{
						Rate = (int)(count / seconds);
						count = 0;startTick = tick;
					}
				}
				Log(LogLevels.Debug, $"Module reached target in {DateTime.Now - startTime}");
				startTime = DateTime.Now;
				WaitHandles(lookupRetryDelay, QuitEvent);
			}

		}
		
		


	}
}
