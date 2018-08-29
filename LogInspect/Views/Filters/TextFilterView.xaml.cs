using LogInspect.Models.Filters;
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
	/// Logique d'interaction pour TextFilterView.xaml
	/// </summary>
	public partial class TextFilterView : UserControl
	{
		public TextFilterView()
		{
			InitializeComponent();
		}

		private void ButtonAdd_Click(object sender, RoutedEventArgs e)
		{
			TextFilterViewModel filter;

			filter = DataContext as TextFilterViewModel;
			if (filter == null) return;
			filter.ItemsSource.Add(new TextFilterItem() { Condition=TextConditions.Equals });
		}

		private void DeleteCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = listBox?.SelectedValue != null;e.Handled = true;
		}

		private void DeleteCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			TextFilterViewModel filter;

			filter = DataContext as TextFilterViewModel;
			if (filter == null) return;
			filter.ItemsSource.Remove((TextFilterItem)listBox.SelectedValue);

		}
	}
}
