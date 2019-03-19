using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.ViewModels.Filters;
using LogInspect.ViewModels.Properties;
using LogInspect.Models;
using LogLib;

namespace LogInspect.ViewModels.Columns
{
	public  class MultiChoicesColumnViewModel : ColumnViewModel
	{
		public override bool AllowsResize => true;
		public override bool AllowsFilter => true;

		public override bool IsImageVisible => false;
		public override string ImageSource => "/LogInspect;component/Images/Calendar.png"; // define a default image souce to avoid converter exceptions


		private FilterItemSourcesViewModel filterItemSourcesViewModel;

		public MultiChoicesColumnViewModel(ILogger Logger, string Name,string Alignment, FilterItemSourcesViewModel FilterChoicesViewModel) : base(Logger,Name,Name,Alignment)
		{

			this.filterItemSourcesViewModel = FilterChoicesViewModel;

		}

		public override PropertyViewModel CreatePropertyViewModel(Event Event)
		{
			return new TextPropertyViewModel(Logger, Name, Event[Name] );
		}
		public override FilterViewModel CreateFilterViewModel()
		{
			return new MultiChoicesFilterViewModel(Logger,Name, filterItemSourcesViewModel.GetFilterChoices(Name),Filter as MultiChoicesFilterViewModel);
		}

	}
}
