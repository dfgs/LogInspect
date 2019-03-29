using LogInspect.Models;
using LogInspect.ViewModels.Columns;
using LogLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.ViewModels.Filters;

namespace LogInspect.ViewModels
{
	public class FilteredEventsViewModel:CollectionViewModel<EventViewModel,EventViewModel>
	{

		public FilterViewModel[] Filters
		{
			get;
			set;
		}

		public FilteredEventsViewModel(ILogger Logger ) : base(Logger)
		{
			//AssertParameterNotNull("Events", Events);
			//this.events = Events;
		}

		private bool MustDiscard(EventViewModel Event)
		{
			if (Filters == null) return false;
			foreach(FilterViewModel filter in Filters)
			{
				if (filter.MustDiscard(Event)) return true;
			}
			return false;
		}

		protected override EventViewModel[] GenerateItems(IEnumerable<EventViewModel> Items)
		{
			return Items.Where((item) => !MustDiscard(item)).ToArray();
		}
		




	


	}
}
