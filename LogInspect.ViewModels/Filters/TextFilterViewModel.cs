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

		public ObservableCollection<TextFilterItem> ItemsSource
		{
			get;
			private set;
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
				ItemsSource.Add(new TextFilterItem() {Condition=TextConditions.Contains }) ;
			}
		}

		public override Filter CreateFilter()
		{
			return new TextFilter(PropertyName) { Items=ItemsSource.ToArray() };
		}



	}
}
