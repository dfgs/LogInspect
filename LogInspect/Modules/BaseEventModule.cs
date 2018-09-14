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
	public abstract class BaseEventModule: ThreadModule
	{
		private ILogParser logParser;

		private int lineIndex;

		public abstract long Position
		{
			get;
		}
		public abstract long Target
		{
			get;
		}

		// lock cause issue here
		public int IndexedEventsCount
		{
			get;
			private set;
		}

		public int Rate
		{
			get;
			private set;
		}

		private int lookupRetryDelay;
		private DateTime startTime;

		public event EventReadEventHandler Read;
		public event EventIndexedEventHandler Indexed;
		public event EventHandler Reseted;

		public BaseEventModule(string Name, ILogger Logger,ILogParser LogParser, ThreadPriority Priority, int LookupRetryDelay) : base(Name, Logger,Priority)
		{
			this.logParser = LogParser;
			this.lookupRetryDelay = LookupRetryDelay;
		}


		protected abstract void OnReset();

		public void Reset()
		{
			IndexedEventsCount = 0;lineIndex = 0;
			OnReset();
			Reseted?.Invoke(this, EventArgs.Empty);
		}

		protected abstract Log OnReadLog();
		protected abstract bool MustIndexEvent(Event Event);
		
		protected override sealed void ThreadLoop()
		{
			Log log;
			Event item;

			startTime = DateTime.Now;

			while(State == ModuleStates.Started)
			{
				while((State == ModuleStates.Started) && (Position < Target))
				{
					try
					{
						log = OnReadLog();
					}
					catch(Exception ex)
					{
						Log(ex);
						return;
					}

					try
					{
						item = logParser.Parse(log);
					}
					catch(Exception ex)
					{
						Log(ex);
						continue;
					}
					if (item==null)
					{
						Log(LogLevels.Warning, $"Cannot parse log: {log.ToSingleLine()}");
					}

					//if (!item.Rule.Discard)
					{
						Read?.Invoke(this, new EventReadEventArgs(item));

						if (MustIndexEvent(item))
						{
							Indexed?.Invoke(this, new EventIndexedEventArgs(item, IndexedEventsCount, lineIndex));
							IndexedEventsCount++;
						}
					}

					lineIndex += log.Lines.Count();

				}
				Log(LogLevels.Debug, $"Indexed reached end of stream in {DateTime.Now - startTime}");
				startTime = DateTime.Now;
				WaitHandles(lookupRetryDelay, QuitEvent);
			}
		}
		
		


	}
}
