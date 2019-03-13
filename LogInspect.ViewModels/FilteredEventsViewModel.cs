using LogInspect.Models;
using LogInspect.Models.Filters;
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

		public Filter[] Filters
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
			foreach(Filter filter in Filters)
			{
				if (filter.MustDiscard(Event[filter.Column].Value)) return true;
			}
			return false;
		}

		protected override IEnumerable<EventViewModel> GenerateItems(IEnumerable<EventViewModel> Items)
		{
			return Items.Where((item) => !MustDiscard(item));
		}
		




	


	}
}
