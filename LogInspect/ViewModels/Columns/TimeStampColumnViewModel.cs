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



		public TimeStampColumnViewModel(ILogger Logger,string Name) : base(Logger,Name)
		{
		}

		public override PropertyViewModel CreatePropertyViewModel(EventViewModel Event)
		{
			return new TimeStampPropertyViewModel(Logger, this, Event);
		}

		public override FilterViewModel CreateFilterViewModel()
		{
			return new TimeStampFilterViewModel(Logger,(TimeStampFilter)Filter);
		}


	}
}
