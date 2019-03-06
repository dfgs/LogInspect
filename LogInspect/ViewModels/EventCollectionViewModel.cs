using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogInspect.ViewModels.Columns;
using LogInspectLib;
using LogLib;

namespace LogInspect.ViewModels
{
	public class EventCollectionViewModel : FilteredCollectionViewModel<Event,EventViewModel>
	{
		private IEnumerable<ColumnViewModel> columns;
		private IEnumerable<EventColoringRule> eventColoringRules;
		public EventCollectionViewModel(ILogger Logger,IEnumerable<ColumnViewModel> Columns, IEnumerable<EventColoringRule> EventColoringRules) : base(Logger)
		{
			AssertParameterNotNull("Columns", Columns);
			AssertParameterNotNull("EventColoringRules", EventColoringRules);
			this.columns = Columns;
			this.eventColoringRules = EventColoringRules;
		}

		protected override IEnumerable<EventViewModel> Filter(IEnumerable<Event> Items)
		{
			return Items.Select((item) => new EventViewModel(Logger, columns ,eventColoringRules, item));
		}


	}
}
