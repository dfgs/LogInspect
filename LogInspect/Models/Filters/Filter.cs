using LogInspect.ViewModels;
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
		public string Column
		{
			get;
			private set;
		}

		public Filter(string Column)
		{
			this.Column = Column;
		}

		public abstract bool MustDiscard(EventViewModel Item);
	}
}
