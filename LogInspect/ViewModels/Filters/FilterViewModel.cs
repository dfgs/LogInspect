using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogInspect.Models.Filters;
using LogLib;

namespace LogInspect.ViewModels.Filters
{
	public abstract class FilterViewModel : ViewModel
	{
		public FilterViewModel(ILogger Logger) : base(Logger)
		{
		}

		public abstract Filter CreateFilter();
		
	}
}
