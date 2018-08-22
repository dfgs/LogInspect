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
	public abstract class BaseEventModule<TInput, TIndexed> : ThreadModule
	{
		private List<TIndexed> items;

		public TIndexed this[int Index]
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

		public int IndexedEventsCount
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

		public event ReadEventHandler<TInput> Read;
		public event IndexedEventHandler<TInput, TIndexed> Indexed;


		public BaseEventModule(string Name, ILogger Logger, ThreadPriority Priority, int LookupRetryDelay) : base(Name, Logger,Priority)
		{
			this.lookupRetryDelay = LookupRetryDelay;
			items = new List<TIndexed>();
		}
		public bool Contains(TIndexed Item)
		{
			lock (items)
			{
				return items.Contains(Item);
			}
		}

		protected virtual void OnReset()
		{
			throw new NotImplementedException("OnReset");
		}
		public void Reset()
		{
			lock(items)
			{
				items.Clear();
				OnReset();
			}
		}

		protected abstract TInput OnReadInput();
		protected abstract bool MustIndexInput(TInput Input);
		protected abstract TIndexed OnCreateIndexItem(TInput Input);
		
		protected override sealed void ThreadLoop()
		{
			TIndexed item;
			TInput input;

			while(State == ModuleStates.Started)
			{
				while((State == ModuleStates.Started) && (Position < Target))
				{
					try
					{
						input = OnReadInput();
						Read?.Invoke(this, new ReadEventArgs<TInput>(input));
					}
					catch(Exception ex)
					{
						Log(ex);
						return;
					}
					if (MustIndexInput(input))
					{
						item= OnCreateIndexItem(input);
						lock (items)
						{
							items.Add(item);
							Indexed?.Invoke(this,new IndexedEventArgs<TInput, TIndexed>(input, item));
						}
					}
				}
				WaitHandles(lookupRetryDelay, QuitEvent);
			}
		}
		
		


	}
}
