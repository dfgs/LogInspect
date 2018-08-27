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
using LogLib;

namespace LogInspect.ViewModels.Columns
{
	public class LineColumnViewModel : ColumnViewModel
	{
		public override bool AllowsResize => false;
		public override bool AllowsFilter => false;

		public override Visibility ImageVisibility => Visibility.Collapsed;
		public override string ImageSource => null;


		public LineColumnViewModel(ILogger Logger,string Name) : base(Logger,Name,Name,"Right")
		{
		}

		public override PropertyViewModel CreatePropertyViewModel(EventViewModel Event)
		{
			return new LinePropertyViewModel(Logger, this, Event);
		}

		public override FilterViewModel CreateFilterViewModel()
		{
			return null;
		}


	}
}
