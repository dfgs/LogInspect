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

		private IEnumerable<EventViewModel> events;


		private Filter[] filters;

		public FilteredEventsViewModel(ILogger Logger , IEnumerable<EventViewModel> Events) : base(Logger)
		{
			AssertParameterNotNull("Events", Events);
			this.events = Events;
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
			Load( events.Where((vm)=>!MustDiscard(vm))  );

		}




	


	}
}
