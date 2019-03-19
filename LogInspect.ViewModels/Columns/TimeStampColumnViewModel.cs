using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.Models;
using LogInspect.Models.Filters;
using LogInspect.ViewModels.Filters;
using LogInspect.ViewModels.Properties;
using LogLib;

namespace LogInspect.ViewModels.Columns
{
	public class TimeStampColumnViewModel : ColumnViewModel
	{
		public override bool AllowsResize => true;
		public override bool AllowsFilter => true;


		public override bool IsImageVisible => true;
		public override string ImageSource => "/LogInspect;component/Images/Calendar.png";

		


		public TimeStampColumnViewModel(ILogger Logger,string Name, string Alignment, string Format) : base(Logger,Name,Name,Alignment)
		{
			this.Format = Format;
		}

		public override PropertyViewModel CreatePropertyViewModel(Event Event)
		{
			DateTime value;
			if (!DateTime.TryParseExact(Event[Name] as string, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out value))
			{
				return new InvalidTimeStampPropertyViewModel(Logger, Name, Event[Name] as string);
			}
			else
			{
				return new TimeStampPropertyViewModel(Logger, Name, value);
			}
		}

		public override FilterViewModel CreateFilterViewModel()
		{
			return new TimeStampFilterViewModel(Logger,Name,(TimeStampFilter)Filter);
		}


	}
}
