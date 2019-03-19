using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.Models;
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
			DateTime result;

			if (DateTime.TryParseExact(Event[Name], Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
			{
				return new TimeStampPropertyViewModel(Logger, Name, result);
			}
			else
			{
				return new InvalidTimeStampPropertyViewModel(Logger, Name, Event[Name]);
			}
		}

		public override FilterViewModel CreateFilterViewModel()
		{
			return new TimeStampFilterViewModel(Logger,Name,Filter as TimeStampFilterViewModel);
		}


	}
}
