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
using System.Windows.Shapes;

namespace LogInspect
{
    /// <summary>
    /// Logique d'interaction pour FilterWindow.xaml
    /// </summary>
    public partial class FilterWindow : Window
    {

		public static readonly DependencyProperty FilterProperty = DependencyProperty.Register("Filter", typeof(FilterViewModel), typeof(FilterWindow));
		public FilterViewModel Filter
		{
			get { return (FilterViewModel)GetValue(FilterProperty); }
			set { SetValue(FilterProperty, value); }
		}

		public bool RemoveFilter
		{
			get;
			private set;
		}

		public FilterWindow()
        {
            InitializeComponent();
        }

		private void ButtonOK_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}
		private void ButtonRemove_Click(object sender, RoutedEventArgs e)
		{
			RemoveFilter = true;
			DialogResult = false;
		}
		private void ButtonCancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}




	}
}
