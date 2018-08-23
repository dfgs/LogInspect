using LogInspect.Models;
using LogInspect.Models.Filters;
using LogInspectLib;
using LogInspectLib.Readers;
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

	public class EventIndexerModule : BaseEventModule<Event,FileIndex>
	{
		private EventReader eventReader;

		private Filter[] filters;

		private int lineIndex;
		private long pos;

		public override long Position
		{
			get { return eventReader.Position; }
		}
		public override long Target
		{
			get { return eventReader.Length; }
		}


		public EventIndexerModule(ILogger Logger, EventReader EventReader,int LookupRetryDelay) : base("EventIndexer",Logger,ThreadPriority.Lowest,LookupRetryDelay)
		{
			lineIndex = 0;pos = 0;
			this.eventReader = EventReader;
		}

		protected override bool MustIndexInput(Event Input)
		{
			if (filters == null) return true;
			foreach (Filter filter in filters)
			{
				if (filter.MustDiscard(Input))
				{
					return false;
				}
			}
			return true;
		}
		protected override Event OnReadInput()
		{

			pos = eventReader.Position;
			return eventReader.Read();

		}
		protected override FileIndex OnCreateIndexItem(Event Input)
		{
			FileIndex fileIndex;

			fileIndex = new FileIndex(pos, lineIndex, IndexedEventsCount);
			lineIndex+=Input.Log.Lines.Length;

			return fileIndex;
		}
		protected override void OnReset()
		{
			eventReader.Seek(0);
		}

		public void SetFilters(IEnumerable<Filter> Filters)
		{
			this.filters = Filters.Select(item => item).ToArray();
			this.Reset();
		}

		




	}
}
