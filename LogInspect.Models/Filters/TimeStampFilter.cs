using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogInspect.Models.Filters
{
    public class TimeStampFilter:Filter
    {
		public static Array Conditions = Enum.GetValues(typeof(TimeStampConditions));

		
		public DateTime StartDate
		{
			get;
			set;
		}


		public DateTime EndDate
		{
			get;
			set;
		}

		public TimeStampConditions Condition
		{
			get;
			set;
		}

		public TimeStampFilter(string PropertyName):base(PropertyName)
		{
		}
		
		public override bool MustDiscard(object Value)
		{

			if (Value is string) return false; // invalid date time
			if (!(Value is DateTime date)) return true;
						
			switch(Condition)
			{
				case TimeStampConditions.After:
					return date < StartDate;
				case TimeStampConditions.Before:
					return date > StartDate;
				case TimeStampConditions.Between:
					return (date < StartDate) || (date > EndDate);
				case TimeStampConditions.On:
					return (date.Date != StartDate.Date) ;
				default:
					return false;
			}
		}

	}
}
