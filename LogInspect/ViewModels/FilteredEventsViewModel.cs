using LogInspect.Models;
using LogInspect.Models.Filters;

using LogInspect.ViewModels.Columns;
using LogInspectLib;
using LogInspectLib.Parsers;
using LogLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace LogInspect.ViewModels
{
	public class FilteredEventsViewModel:CollectionViewModel<EventViewModel>
	{

		private IEnumerable<ColumnViewModel> columns;
		private IEnumerable<EventColoringRule> eventColoringRules;
		private IEnumerable<Event> events;


		private Filter[] filters;

		public FilteredEventsViewModel(ILogger Logger , IEnumerable<Event> Events, IEnumerable<ColumnViewModel> Columns, IEnumerable<EventColoringRule> EventColoringRules) : base(Logger)
		{
			AssertParameterNotNull("Events", Events);
			AssertParameterNotNull("Columns", Columns);
			AssertParameterNotNull("EventColoringRules", EventColoringRules);
			this.events = Events;
			this.columns = Columns;
			this.eventColoringRules = EventColoringRules;
		}

		private bool MustDiscard(EventViewModel Event)
		{
			if (filters == null) return false;
			foreach(Filter filter in filters)
			{
				if (filter.MustDiscard(Event)) return true;
			}
			return false;
		}
		public void Refresh(Filter[] Filters)
		{

			this.filters = Filters;
			Load( events.Select((ev)=>new EventViewModel(Logger, columns, eventColoringRules, ev)).Where((vm)=>!MustDiscard(vm))  );

		}




	


	}
}
