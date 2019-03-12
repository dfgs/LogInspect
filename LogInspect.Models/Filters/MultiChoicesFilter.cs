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
		public string[] FilteredItems
		{
			get;
			set;
		}

		public MultiChoicesFilter(string PropertyName):base(PropertyName)
		{
	
		}

		public override bool MustDiscard(object Value)
		{
			if (!(Value is string value)) return false;
			return FilteredItems.Contains( value );
		}

	}
}
