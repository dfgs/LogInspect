using LogInspect.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LogInspect.Views
{
	/// <summary>
	/// Logique d'interaction pour EventView.xaml
	/// </summary>
	public partial class EventView : UserControl
	{

		public EventView()
		{
			InitializeComponent();
		}

		private void ListView_Selected(object sender, SelectionChangedEventArgs e)
		{
			ListView listView;
			listView = sender as ListView;
			if (listView == null) return;
			listView.ScrollIntoView(listView.SelectedItem);
		}

		private void ListView_Click(object sender, RoutedEventArgs e)
		{
			ListView listView;
			GridViewColumnHeader columnHeader;
			ColumnViewModel column;
			LogFileViewModel logFile;
			FilterWindow window;
			bool result;

			listView = sender as ListView;
			if (listView == null) return;

			logFile = listView.DataContext as LogFileViewModel;
			if (logFile == null) return;

			columnHeader = e.OriginalSource as GridViewColumnHeader;
			if (columnHeader == null) return;
			
			column = columnHeader.Content as ColumnViewModel;
			if (column == null) return;

			if (!column.AllowsFilter) return;

			window = new FilterWindow(); window.Owner = Application.Current.MainWindow; window.Filter = column.CreateFilterViewModel();
			result = window.ShowDialog()??false;

			if (!(window.RemoveFilter || result)) return;

			if (result == true) column.Filter = window.Filter.CreateFilter();
			else column.Filter = null;

			logFile.Refresh();

		}
	}
}
