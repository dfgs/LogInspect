using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LogInspect.Models;
using LogInspect.Models.Filters;
using LogInspect.Modules;
using LogInspect.ViewModels.Filters;
using LogInspect.ViewModels.Properties;
using LogInspect.Views;
using LogInspectLib;
using LogLib;

namespace LogInspect.ViewModels.Columns
{
	public class TimeStampColumnViewModel : ColumnViewModel
	{
		public override bool AllowsResize => true;
		public override bool AllowsFilter => true;


		public override Visibility ImageVisibility => Visibility.Visible;
		public override string ImageSource => "/LogInspect;component/Images/Calendar.png";



		public TimeStampColumnViewModel(ILogger Logger,string Name, string Alignment) : base(Logger,Name,Name,Alignment)
		{
		
		}

		public override PropertyViewModel CreatePropertyViewModel(EventViewModel Event)
		{
			return new TimeStampPropertyViewModel(Logger, this, Event);
		}

		public override FilterViewModel CreateFilterViewModel()
		{
			return new TimeStampFilterViewModel(Logger,Name,(TimeStampFilter)Filter);
		}


	}
}
