using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogInspect.Modules;
using LogInspect.ViewModels.Columns;
using LogInspectLib;
using LogLib;

namespace LogInspect.ViewModels
{
	public class EventCollectionViewModel : CollectionViewModel<Event,EventViewModel>
	{
		private IEnumerable<ColumnViewModel> columns;
		public EventCollectionViewModel(ILogger Logger,IEnumerable<ColumnViewModel> Columns) : base(Logger)
		{
			AssertParameterNotNull("Columns", Columns);
			this.columns = Columns;
		}
		

		protected override IEnumerable<EventViewModel> GenerateItems(IEnumerable<Event> Items)
		{
			return Items.Select((item) => new EventViewModel(Logger,  columns.Select((column)=> column.CreatePropertyViewModel(item) )));
		}


	}
}
