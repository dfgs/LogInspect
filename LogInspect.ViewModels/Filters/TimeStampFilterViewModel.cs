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
		public static Array Conditions = Enum.GetValues(typeof(TimeStampConditions));

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




		public TimeStampFilterViewModel(ILogger Logger,string PropertyName,TimeStampFilterViewModel Model) : base(Logger,PropertyName)
		{
			if (Model!=null)
			{
				this.StartDate = Model.StartDate;
				this.StartTime = Model.StartTime;
				this.EndDate = Model.EndDate;
				this.EndTime = Model.EndTime;
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

		public override bool MustDiscard(EventViewModel Event)
		{
			if (!(Event[PropertyName].Value is DateTime date) ) return false;
			switch (Condition)
			{
				case TimeStampConditions.After:return date < StartDate + StartTime;
				case TimeStampConditions.Before:return date > StartDate + StartTime;
				case TimeStampConditions.On: return date.Date == StartDate;
				case TimeStampConditions.Between: return (date < StartDate + StartTime) || (date > EndDate + EndTime);
				default:return false;
			}
		}

		/*public override Filter CreateFilter()
		{
			return new TimeStampFilter(PropertyName) { StartDate = StartDate+StartTime, EndDate = EndDate + EndTime, Condition = Condition };
		}*/

	}
}
