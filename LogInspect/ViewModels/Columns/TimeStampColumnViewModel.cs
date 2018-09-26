using System;
using System.Collections.Generic;
using System.Globalization;
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
using LogInspectLib.Parsers;
using LogLib;

namespace LogInspect.ViewModels.Columns
{
	public class TimeStampColumnViewModel : ColumnViewModel
	{
		public override bool AllowsResize => true;
		public override bool AllowsFilter => true;


		public override Visibility ImageVisibility => Visibility.Visible;
		public override string ImageSource => "/LogInspect;component/Images/Calendar.png";



		public TimeStampColumnViewModel(ILogger Logger,string Name, string Alignment, string Format) : base(Logger,Name,Name,Alignment,Format)
		{
		
		}

		public override PropertyViewModel CreatePropertyViewModel(EventViewModel Event)
		{
			DateTime value;
			DateTime.TryParseExact(Event.GetEventValue(Name), Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out value) ;
			return new TimeStampPropertyViewModel(Logger, Name,Alignment, value);
		}

		public override FilterViewModel CreateFilterViewModel()
		{
			return new TimeStampFilterViewModel(Logger,Name,(TimeStampFilter)Filter);
		}


	}
}
