using LogInspect.ViewModels;
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

namespace LogInspect.Views
{
	/// <summary>
	/// Logique d'interaction pour FindView.xaml
	/// </summary>
	public partial class FindView : UserControl
	{




		public FindView()
		{
			InitializeComponent();
			
		}

		/*protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			base.OnPreviewKeyDown(e);
		}*/

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if ((e.Property==VisibilityProperty) && (ValueType.Equals(e.NewValue,Visibility.Visible)))
			{
				textBox.Focus();
			}
		}

		#region commands
		private void CloseCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			LogFileViewModel vm;
			vm = DataContext as LogFileViewModel;
			e.CanExecute = (vm != null) ; e.Handled = true;
		}

		private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			LogFileViewModel vm;
			vm = DataContext as LogFileViewModel;

			vm.FindOptions.IsVisible = false;
		}



		#endregion

		

	}
}
