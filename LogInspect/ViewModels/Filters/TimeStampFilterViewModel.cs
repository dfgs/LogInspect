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


		public static readonly DependencyProperty StartTimeProperty = DependencyProperty.Register("StartTime", typeof(TimeSpan), typeof(TimeStampFilterViewModel));
		public TimeSpan StartTime
		{
			get { return (TimeSpan)GetValue(StartTimeProperty); }
			set { SetValue(StartTimeProperty, value); }
		}


		public static readonly DependencyProperty EndDateProperty = DependencyProperty.Register("EndDate", typeof(DateTime), typeof(TimeStampFilterViewModel));
		public DateTime EndDate
		{
			get { return (DateTime)GetValue(EndDateProperty); }
			set { SetValue(EndDateProperty, value); }
		}


		public static readonly DependencyProperty EndTimeProperty = DependencyProperty.Register("EndTime", typeof(TimeSpan), typeof(TimeStampFilterViewModel));
		public TimeSpan EndTime
		{
			get { return (TimeSpan)GetValue(EndTimeProperty); }
			set { SetValue(EndTimeProperty, value); }
		}



		public static readonly DependencyProperty ConditionProperty = DependencyProperty.Register("Condition", typeof(TimeStampConditions), typeof(TimeStampFilterViewModel));
		public TimeStampConditions Condition
		{
			get { return (TimeStampConditions)GetValue(ConditionProperty); }
			set { SetValue(ConditionProperty, value); }
		}

		


		public TimeStampFilterViewModel(ILogger Logger,string PropertyName,TimeStampFilter Model) : base(Logger,PropertyName)
		{
			if (Model!=null)
			{
				this.StartDate = Model.StartDate.Date;
				this.StartTime = Model.StartDate.TimeOfDay;
				this.EndDate = Model.EndDate.Date;
				this.EndTime = Model.EndDate.TimeOfDay;
				this.Condition = Model.Condition;
			}
			else
			{
				this.StartDate = DateTime.Now.Date;
				this.EndDate = DateTime.Now.Date;
				this.StartTime = DateTime.Now.TimeOfDay;
				this.EndTime = DateTime.Now.TimeOfDay;
			}
		}

		public override Filter CreateFilter()
		{
			return new TimeStampFilter(PropertyName) { StartDate = StartDate+StartTime, EndDate = EndDate + EndTime, Condition = Condition };
		}

	}
}
