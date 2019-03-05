using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LogInspect.Models.Filters;

using LogInspect.ViewModels.Filters;
using LogInspect.ViewModels.Properties;
using LogInspect.Views;
using LogInspectLib;
using LogInspectLib.Parsers;
using LogLib;

namespace LogInspect.ViewModels.Columns
{
	public class MultiChoicesColumnViewModel : TextPropertyColumnViewModel
	{
		public override bool AllowsResize => true;
		public override bool AllowsFilter => true;

		public override Visibility ImageVisibility => Visibility.Collapsed;
		public override string ImageSource => "/LogInspect;component/Images/Calendar.png"; // define a default image souce to avoid converter exceptions


		private FilterItemSourcesViewModel filterItemSourcesViewModel;

		public MultiChoicesColumnViewModel(ILogger Logger, string Name,string Alignment, IInlineParser InlineParser, FilterItemSourcesViewModel FilterChoicesViewModel) : base(Logger,Name,Alignment,  InlineParser)
		{

			this.filterItemSourcesViewModel = FilterChoicesViewModel;

		}

		

		public override FilterViewModel CreateFilterViewModel()
		{
			return new MultiChoicesFilterViewModel(Logger,Name, filterItemSourcesViewModel.GetFilterChoices(Name),(MultiChoicesFilter)Filter);
		}

	}
}
