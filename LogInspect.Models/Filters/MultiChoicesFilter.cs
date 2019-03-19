using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogInspect.Models.Filters
{
    public class MultiChoicesFilter:Filter
    {
		

		// must not be enumerable to avoid dispatcher issues
		public object[] FilteredItems
		{
			get;
			set;
		}

		public MultiChoicesFilter(string PropertyName):base(PropertyName)
		{
	
		}

		public override bool MustDiscard(object Value)
		{
			return FilteredItems.Contains( Value );
		}

	}
}
