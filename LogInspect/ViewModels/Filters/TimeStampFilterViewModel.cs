using LogInspect.Models.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using LogLib;

namespace LogInspect.ViewModels.Filters
{
	public class TimeStampFilterViewModel:FilterViewModel
	{
		

		public static readonly DependencyProperty StartDateProperty = DependencyProperty.Register("StartDate", typeof(DateTime), typeof(TimeStampFilterViewModel));
		public DateTime StartDate
		{
			get { return (DateTime)GetValue(StartDateProperty); }
			set { SetValue(StartDateProperty, value); }
		}


		public static readonly DependencyProperty EndDateProperty = DependencyProperty.Register("EndDate", typeof(DateTime), typeof(TimeStampFilterViewModel));
		public DateTime EndDate
		{
			get { return (DateTime)GetValue(EndDateProperty); }
			set { SetValue(EndDateProperty, value); }
		}

		public TimeStampFilterViewModel(ILogger Logger,TimeStampFilter Model) : base(Logger)
		{
			if (Model!=null)
			{
				this.StartDate = Model.StartDate;
				this.EndDate = Model.EndDate;
			}
		}

		public override Filter CreateFilter()
		{
			return new TimeStampFilter() { StartDate = StartDate, EndDate = EndDate };
		}

	}
}
