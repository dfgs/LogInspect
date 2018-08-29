using LogInspect.ViewModels.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LogInspect.Views.Filters
{
	/// <summary>
	/// Logique d'interaction pour SelectionFilterView.xaml
	/// </summary>
	public partial class MultiChoicesFilterView : UserControl
	{
		public MultiChoicesFilterView()
		{
			InitializeComponent();
		}

		private void AllButton_Click(object sender, RoutedEventArgs e)
		{
			MultiChoicesFilterViewModel filter;

			filter = DataContext as MultiChoicesFilterViewModel;
			if (filter == null) return;
			foreach(FilterItemViewModel item in filter.ItemsSource)
			{
				item.IsChecked = true;
			}
		}
		private void NoneButton_Click(object sender, RoutedEventArgs e)
		{
			MultiChoicesFilterViewModel filter;

			filter = DataContext as MultiChoicesFilterViewModel;
			if (filter == null) return;
			foreach (FilterItemViewModel item in filter.ItemsSource)
			{
				item.IsChecked = false;
			}
		}
		private void InvertButton_Click(object sender, RoutedEventArgs e)
		{
			MultiChoicesFilterViewModel filter;

			filter = DataContext as MultiChoicesFilterViewModel;
			if (filter == null) return;
			foreach (FilterItemViewModel item in filter.ItemsSource)
			{
				item.IsChecked = !item.IsChecked;
			}
		}

	}
}
