using LogInspectLib;
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

		public override bool MustDiscard(Event Item)
		{
			return FilteredItems.Contains( Item[PropertyName] );
		}

	}
}
