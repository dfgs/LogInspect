using LogInspect.Models;
using LogInspect.Models.Filters;
using LogInspectLib;
using LogInspectLib.Parsers;
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
		private ILogReader logReader;

		private Filter[] filters;

		public override long Position
		{
			get { return logReader.Position; }
		}
		public override long Target
		{
			get { return logReader.Length; }
		}


		public EventIndexerModule(ILogger Logger, ILogReader LogReader,ILogParser LogParser, int LookupRetryDelay) : base("EventIndexer",Logger,LogParser, ThreadPriority.Lowest,LookupRetryDelay)
		{
			this.logReader = LogReader;
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

		protected override Log OnReadLog()
		{
			return logReader.Read();

		}

		protected override void OnReset()
		{
			logReader.Seek(0);
		}

		public void SetFilters(IEnumerable<Filter> Filters)
		{
			this.filters = Filters.Select(item => item).ToArray();
			this.Reset();
		}

		




	}
}
