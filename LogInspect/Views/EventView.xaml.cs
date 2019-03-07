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

		private async void ListView_Click(object sender, RoutedEventArgs e)
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

			await logFile.Refresh();

		}

		private void Marker_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Border border;
			MarkerViewModel marker;
			LogFileViewModel logFileViewModel;

			logFileViewModel = DataContext as LogFileViewModel;
			if (logFileViewModel == null) return;

			border = sender as Border;
			if (border == null) return;

			marker = border.DataContext as MarkerViewModel;
			if (marker == null) return;

			logFileViewModel.FilteredEvents.Select(marker.Position);
		}

		private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;e.Handled = true;
		}

		private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ListView listView;
			GridView gridView;

			listView = sender as ListView;
			if (listView == null) return;
			gridView = listView.View as GridView;
			if (gridView == null) return;

			StringBuilder buffer = new StringBuilder();
			
			foreach(ColumnViewModel column in gridView.Columns.Select(item=>item.Header))
			{
				if (string.IsNullOrEmpty(column.Description)) continue; 
				buffer.Append(column.Description);
				buffer.Append("\t");
			}

			buffer.Append("\n");
			
			foreach (EventViewModel ev in listView.SelectedItems)
			{
				foreach (ColumnViewModel column in gridView.Columns.Select(item => item.Header))
				{
					if (string.IsNullOrEmpty(column.Description)) continue;
					buffer.Append(ev[column.Name].Value);
					buffer.Append("\t");
				}
				buffer.Append("\n");
			}

		

			Clipboard.SetText(buffer.ToString());
		}




	}
}
