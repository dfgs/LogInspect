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
		public override bool MustDiscard(EventViewModel Item)
		{
			string value;

			try
			{
				value = Item[Column].ToString();
			}
			catch
			{
				return false;
			}
			foreach (TextFilterItem item in Items)
			{
				if (item.Match(value)) return false;
			}

			return true;

		}

	}
}
