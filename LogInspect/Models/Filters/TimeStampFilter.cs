using LogInspect.ViewModels;
using LogInspect.ViewModels.Properties;
using LogInspectLib;
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
		
		public override bool MustDiscard(EventViewModel Item)
		{
			DateTime date;

			if (Item[Column].Value is string) return false; // invalid date time

			try
			{
				date = (DateTime)Item[Column].Value;
			}
			catch
			{
				return false;
			}

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
