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
		

		public DateTime StartDate
		{
			get;
			set;
		}


		public TimeSpan StartTime
		{
			get;
			set;
		}


		public DateTime EndDate
		{
			get;
			set;
		}


		public TimeSpan EndTime
		{
			get;
			set;
		}



		public TimeStampConditions Condition
		{
			get;
			set;
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
