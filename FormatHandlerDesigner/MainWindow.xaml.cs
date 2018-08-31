using Microsoft.Win32;
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

namespace FormatHandlerDesigner
{
	/// <summary>
	/// Logique d'interaction pour MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private AppViewModel appViewModel;

		public MainWindow()
		{
			appViewModel = new AppViewModel();
			InitializeComponent();
			DataContext = appViewModel;
		}

		private void ShowError(Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK);
		}
		private void OpenCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true; e.Handled = true;
		}

		private void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			OpenFileDialog dialog;

			dialog = new OpenFileDialog();
			dialog.Title = "Open xml file";
			dialog.Filter = "xml files|*.xml|All files|*.*";

			if (dialog.ShowDialog(this) ?? false)
			{
				try
				{
					appViewModel.Open(dialog.FileName);
				}
				catch(Exception ex)
				{
					ShowError(ex);
				}
			}
		}
		private void SaveCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = appViewModel.SelectedItem!=null; e.Handled = true;
		}

		private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			SaveFileDialog dialog;

			dialog = new SaveFileDialog();
			dialog.Title = "Save xml file";
			dialog.Filter = "xml files|*.xml|All files|*.*";

			if (dialog.ShowDialog(this) ?? false)
			{
				try
				{
					appViewModel.SelectedItem.SaveToFile(dialog.FileName);
				}
				catch (Exception ex)
				{
					ShowError(ex);
				}
			}
		}


		private void NewCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true; e.Handled = true;
		}

		private void NewCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			appViewModel.CreateNew();
			
		}

		private void CloseCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem != null) ; e.Handled = true;
		}

		private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			appViewModel.CloseCurrent();
		}
	}
}
