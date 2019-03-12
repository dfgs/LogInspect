using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogInspect.Modules;
using LogInspect.ViewModels.Columns;
using LogInspect.Models;
using LogLib;

namespace LogInspect.ViewModels
{
	public class EventCollectionViewModel : CollectionViewModel<Event,EventViewModel>
	{
		private IColorProviderModule colorProviderModule;
		private IEnumerable<ColumnViewModel> columns;
		public EventCollectionViewModel(ILogger Logger, IColorProviderModule ColorProviderModule,IEnumerable<ColumnViewModel> Columns) : base(Logger)
		{
			AssertParameterNotNull("ColorProviderModule", ColorProviderModule);
			AssertParameterNotNull("Columns", Columns);
			this.colorProviderModule = ColorProviderModule;
			this.columns = Columns;
		}
		

		protected override IEnumerable<EventViewModel> GenerateItems(IEnumerable<Event> Items)
		{
			return Items.Select((item) => new EventViewModel(Logger, colorProviderModule.GetBackground(item), columns.Select((column)=> column.CreatePropertyViewModel(item) )));
		}


	}
}
