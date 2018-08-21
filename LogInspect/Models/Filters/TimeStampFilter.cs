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


		
		public override bool MustDiscard(FileIndex Item)
		{
			return (Item.TimeStamp < StartDate) || (Item.TimeStamp > EndDate);
		}

	}
}
