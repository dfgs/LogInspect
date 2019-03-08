using LogInspect.ViewModels.Columns;
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
	/// Logique d'interaction pour ColumnsFormatWindow.xaml
	/// </summary>
	public partial class ColumnsFormatWindow : Window
	{


		public static readonly DependencyProperty ColumnFormatViewModelsProperty = DependencyProperty.Register("ColumnFormatViewModels", typeof(IEnumerable<ColumnFormatViewModel>), typeof(ColumnsFormatWindow));
		public IEnumerable<ColumnFormatViewModel> ColumnFormatViewModels
		{
			get { return (IEnumerable<ColumnFormatViewModel>)GetValue(ColumnFormatViewModelsProperty); }
			set { SetValue(ColumnFormatViewModelsProperty, value); }
		}

		public ColumnsFormatWindow()
		{
			InitializeComponent();
		}

		private void ButtonOK_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}
		
		private void ButtonCancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}


	}
}
