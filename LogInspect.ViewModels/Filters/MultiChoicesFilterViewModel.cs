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


		



		public MultiChoicesFilterViewModel(ILogger Logger,string PropertyName, IEnumerable<object> ItemsSource, MultiChoicesFilterViewModel Model):base(Logger,PropertyName)
		{
			List<FilterItemViewModel> items;
			items = new List<FilterItemViewModel>();
			if (Model == null)
			{
				foreach (object value in ItemsSource)
				{
					items.Add(new FilterItemViewModel() { Value = value.ToString(), IsChecked = true });
				}
			}
			else
			{
				foreach(FilterItemViewModel item in Model.ItemsSource)
				{
					items.Add(new FilterItemViewModel() { Value =item.Value, IsChecked = item.IsChecked});
				}
			}
			this.ItemsSource = items;
		}

		

		public override bool MustDiscard(EventViewModel Event)
		{
			FilterItemViewModel item;

			item = ItemsSource.FirstOrDefault(item2 =>ValueType.Equals( item2.Value , Event[PropertyName].Value));
			return !item?.IsChecked ?? false;
		}


	}
}
