using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogLib;

namespace LogInspect.ViewModels.Filters
{
	public abstract class FilterViewModel : ViewModel
	{
		public string PropertyName
		{
			get;
			private set;
		}

		public FilterViewModel(ILogger Logger,string PropertyName) : base(Logger)
		{
			this.PropertyName = PropertyName;
		}

		//public abstract FilterViewModel CreateFilter();

		public abstract bool MustDiscard(EventViewModel Event);

	}
}
