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

	public class EventIndexerModule : BaseEventModule
	{
		private EventReader eventReader;

		private Filter[] filters;

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
			this.eventReader = EventReader;
		}

		protected override bool MustIndexEvent(Event Input)
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
		protected override Event OnReadEvent()
		{
			return eventReader.Read();

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
