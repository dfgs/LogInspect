using LogInspect.Models.Filters;
using LogLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogInspect.ViewModels.Filters
{
	public class TextFilterViewModel:FilterViewModel
	{

		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<TextFilterItem>), typeof(TextFilterViewModel));
		public ObservableCollection<TextFilterItem> ItemsSource
		{
			get { return (ObservableCollection<TextFilterItem>)GetValue(ItemsSourceProperty); }
			private set { SetValue(ItemsSourceProperty, value); }
		}

		public TextFilterViewModel(ILogger Logger, string PropertyName, TextFilter Model):base(Logger,PropertyName)
		{
			ItemsSource = new ObservableCollection<TextFilterItem>();
			if (Model != null)
			{
				foreach (TextFilterItem item in Model.Items)
				{
					ItemsSource.Add(new TextFilterItem() {  Condition=item.Condition, Value=item.Value });
				}
			}
			else
			{
				ItemsSource.Add(new TextFilterItem() {Condition=TextConditions.Equals }) ;
			}
		}

		public override Filter CreateFilter()
		{
			return new TextFilter(PropertyName) { Items=ItemsSource.ToArray() };
		}



	}
}
