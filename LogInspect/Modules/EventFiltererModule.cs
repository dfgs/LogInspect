using LogInspect.Models;
using LogInspect.Models.Filters;
using LogLib;
using ModuleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace LogInspect.Modules
{
	public class EventFiltererModule : BaseEventModule<FileIndex>
	{
		private EventIndexerModule indexerModule;

		private Filter[] filters;

		private int position;
		public override long Position
		{
			get { return position; }
		}

		public override long Target
		{
			get { return indexerModule.IndexedEventsCount; }
		}

		public EventFiltererModule( ILogger Logger,EventIndexerModule IndexerModule,int LookupRetryDelay) : base("EventFilterer", Logger,ThreadPriority.Lowest,LookupRetryDelay)
		{
			position = 0;
			this.indexerModule = IndexerModule;
		}

		protected override bool MustIndexItem(FileIndex Item)
		{
			if (filters == null) return true;
			foreach (Filter filter in filters)
			{
				if ( filter.MustDiscard(Item))
				{
					return false;
				}
			}
			return true;
		}
		protected override FileIndex OnReadItem()
		{
			FileIndex fileIndex;

			fileIndex=indexerModule[position];
			position++;
			return fileIndex;
		}
		protected override void OnReset()
		{
			position = 0;
		}

		public void SetFilters(IEnumerable<Filter> Filters)
		{
			this.filters = Filters.Select(item=>item).ToArray();
			this.Reset();
		}

		public bool FindPrevious(ref int Index, Func<FileIndex, bool> Predicate)
		{
			FileIndex fileIndex;

			while (Index > 0)
			{
				Index--;
				fileIndex = this[Index];
				if (Predicate(fileIndex)) return true;
			}
			return false;
		}
		public bool FindNext(ref int Index, Func<FileIndex, bool> Predicate)
		{
			FileIndex fileIndex;

			while (Index < IndexedEventsCount - 1)
			{
				Index++;
				fileIndex = this[Index];
				if (Predicate(fileIndex)) return true;
			}
			return false;
		}


	}
}
