using LogInspect.Models.Filters;
using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogInspect.ViewModels.Filters
{
	public class MultiChoicesFilterViewModel:FilterViewModel
	{

		public IEnumerable<FilterItemViewModel> ItemsSource
		{
			get;
			private set;
		}


		



		public MultiChoicesFilterViewModel(ILogger Logger,string PropertyName, IEnumerable<object> ItemsSource, MultiChoicesFilter Model):base(Logger,PropertyName)
		{
			List<FilterItemViewModel> items;
			items = new List<FilterItemViewModel>();
			foreach(string value in ItemsSource)
			{
				items.Add(new FilterItemViewModel() { Description=value,IsChecked=!(Model?.FilteredItems.Contains(value)??false) });
			}
			this.ItemsSource = items;
		}

		public override Filter CreateFilter()
		{
			return new MultiChoicesFilter(PropertyName) { FilteredItems = ItemsSource.Where(item => !item.IsChecked).Select(item=>item.Description).ToArray()  };
		}



	}
}
