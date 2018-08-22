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
	public class SelectionFilterViewModel:FilterViewModel
	{

		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable<SelectionFilterItem>), typeof(SelectionFilterViewModel));
		public IEnumerable<SelectionFilterItem> ItemsSource
		{
			get { return (IEnumerable<SelectionFilterItem>)GetValue(ItemsSourceProperty); }
			private set { SetValue(ItemsSourceProperty, value); }
		}


		public string Property
		{
			get;
			private set;
		}



		public SelectionFilterViewModel(ILogger Logger,string Property, IEnumerable<object> ItemsSource, SelectionFilter Model):base(Logger)
		{
			List<SelectionFilterItem> items;
			this.Property = Property;
			items = new List<SelectionFilterItem>();
			foreach(string value in ItemsSource)
			{
				items.Add(new SelectionFilterItem() { Description=value,IsChecked=!(Model?.FilteredItems.Contains(value)??false) });
			}
			this.ItemsSource = items;
		}

		public override Filter CreateFilter()
		{
			return new SelectionFilter(Property) { FilteredItems = ItemsSource.Where(item => !item.IsChecked).Select(item=>item.Description)  };
		}



	}
}
