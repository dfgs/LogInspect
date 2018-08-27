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
	public class MultiChoicesColumnViewModel : ColumnViewModel
	{
		public override bool AllowsResize => true;
		public override bool AllowsFilter => true;

		public override Visibility ImageVisibility => Visibility.Collapsed;
		public override string ImageSource => null;


		private FilterItemSourcesViewModel filterItemSourcesViewModel;

		public MultiChoicesColumnViewModel(ILogger Logger, string Name,string Alignment, FilterItemSourcesViewModel FilterChoicesViewModel) : base(Logger,Name,Name,Alignment)
		{

			this.filterItemSourcesViewModel = FilterChoicesViewModel;

		}

		public override PropertyViewModel CreatePropertyViewModel(EventViewModel Event)
		{
			return new TextPropertyViewModel(Logger, this, Event);
		}

		public override FilterViewModel CreateFilterViewModel()
		{
			return new MultiChoicesFilterViewModel(Logger,Name, filterItemSourcesViewModel.GetFilterChoices(Name),(MultiChoicesFilter)Filter);
		}

	}
}
