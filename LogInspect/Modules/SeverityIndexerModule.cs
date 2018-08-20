using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LogInspect.Models;
using LogLib;

namespace LogInspect.Modules
{
	public class SeverityIndexerModule : BaseEventModule<string>
	{
		private EventIndexerModule indexerModule;

		private int position;
		public override long Position
		{
			get { return position; }
		}

		public override long Target
		{
			get { return indexerModule.IndexedEventsCount; }
		}

		public SeverityIndexerModule(ILogger Logger, EventIndexerModule IndexerModule, int LookupRetryDelay) : base("SeverityIndexer", Logger, ThreadPriority.Lowest, LookupRetryDelay)
		{
			position = 0;
			this.indexerModule = IndexerModule;
		}
		protected override string OnReadItem()
		{
			FileIndex fileIndex;

			fileIndex = indexerModule[position];
			position++;
			return fileIndex.Severity;
		}
		protected override bool MustIndexItem(string Item)
		{
			return !Contains(Item);
		}

		


	}
}
