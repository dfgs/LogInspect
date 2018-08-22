using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LogInspect.Models.Filters;
using LogInspect.Modules;
using LogInspect.ViewModels.Filters;
using LogInspect.ViewModels.Properties;
using LogInspect.Views;
using LogLib;

namespace LogInspect.ViewModels.Columns
{
	public class SelectionPropertyColumnViewModel : ColumnViewModel
	{
		public override bool AllowsResize => true;
		public override bool AllowsFilter => true;



		private SelectionFiltersIndexerModule selectionFiltersIndexerModule;

		public SelectionPropertyColumnViewModel(ILogger Logger, string Name,string Alignment,SelectionFiltersIndexerModule SelectionFiltersIndexerModule) : base(Logger,Name,Alignment)
		{

			this.selectionFiltersIndexerModule = SelectionFiltersIndexerModule;

		}

		public override PropertyViewModel CreatePropertyViewModel(EventViewModel Event)
		{
			return new TextPropertyViewModel(Logger, this, Event,Alignment);
		}

		public override FilterViewModel CreateFilterViewModel()
		{
			return new SelectionFilterViewModel(Logger,Name, selectionFiltersIndexerModule.GetItemsSource(Name),(SelectionFilter)Filter);
		}

	}
}
