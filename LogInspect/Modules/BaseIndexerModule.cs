using LogInspect.Models;
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
	public abstract class BaseIndexerModule<T> : ThreadModule
	{
		private List<T> items;

		public T this[int Index]
		{
			get { return items[Index]; }
		}
		public abstract long Position
		{
			get;
		}
		public abstract long Target
		{
			get;
		}

		public int IndexedEvents
		{
			get
			{
				lock (items)
				{
					return items.Count;
				}
			}
		}

		private int lookupRetryDelay;

		public BaseIndexerModule(string Name, ILogger Logger, ThreadPriority Priority, int LookupRetryDelay) : base(Name, Logger,Priority)
		{
			this.lookupRetryDelay = LookupRetryDelay;
			items = new List<T>();
		}
		public bool Contains(T Item)
		{
			lock (items)
			{
				return items.Contains(Item);
			}
		}

		protected abstract T OnReadItem();
		protected abstract bool MustIndexItem(T Item);

		protected override sealed void ThreadLoop()
		{
			T item;

			while(State == ModuleStates.Started)
			{
				while((State == ModuleStates.Started) && (Position < Target))
				{
					try
					{
						item = OnReadItem();
					}
					catch(Exception ex)
					{
						Log(ex);
						return;
					}
					if (MustIndexItem(item))
					{
						lock (items)
						{
							items.Add(item);
						}
					}
				}
				WaitHandles(lookupRetryDelay, QuitEvent);
			}
		}
		
		


	}
}
