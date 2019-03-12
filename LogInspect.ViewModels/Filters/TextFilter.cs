using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogInspect.Models.Filters
{
    public class TextFilter:Filter
    {
		public TextFilterItem[] Items
		{
			get;
			set;
		}

		public TextFilter(string PropertyName):base(PropertyName)
		{
			
		}
		public override bool MustDiscard(object Value)
		{
			if (Value == null) return false;
			string value = Value.ToString();

			foreach (TextFilterItem item in Items)
			{
				if (item.Match(value)) return false;
			}

			return true;

		}

	}
}
