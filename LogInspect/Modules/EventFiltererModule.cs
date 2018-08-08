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
	public class EventFiltererModule : BaseIndexerModule<FileIndex>
	{
		private EventIndexerModule indexerModule;

		private IEnumerable<string> severities;

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
			if (severities == null) return true;
			if (severities.Contains(Item.Severity)) return false;
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

		public void SetFilter(IEnumerable<string> Severities)
		{
			this.severities = Severities;
			this.Reset();
		}


	}
}
