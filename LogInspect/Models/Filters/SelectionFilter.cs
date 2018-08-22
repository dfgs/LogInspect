using LogInspectLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogInspect.Models.Filters
{
    public class SelectionFilter:Filter
    {
		public string Property
		{
			get;
			private set;
		}
		public IEnumerable<string> FilteredItems
		{
			get;
			set;
		}

		public SelectionFilter(string Property)
		{
			this.Property = Property;
		}

		public override bool MustDiscard(Event Item)
		{
			return FilteredItems.Contains( Item.GetValue(Property) );
		}

	}
}
