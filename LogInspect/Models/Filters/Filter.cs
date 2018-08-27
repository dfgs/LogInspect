using LogInspectLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogInspect.Models.Filters
{
    public abstract class Filter
    {
		public string PropertyName
		{
			get;
			private set;
		}

		public Filter(string PropertyName)
		{
			this.PropertyName = PropertyName;
		}

		public abstract bool MustDiscard(Event Item);
	}
}
