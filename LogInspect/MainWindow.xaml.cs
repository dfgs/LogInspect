using LogInspect.ViewModels;
using LogInspectLib;
using LogLib;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace LogInspect
{
	/// <summary>
	/// Logique d'interaction pour MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{


		private ILogger logger;
		private AppViewModel appViewModel;

		public MainWindow()
		{

			logger = new FileLogger(new DefaultLogFormatter(),"LogInspect.log");
			appViewModel = new AppViewModel(logger, Properties.Settings.Default.FormatHandlersFolder, Properties.Settings.Default.PatternLibsFolder, Properties.Settings.Default.InlineColoringRuleLibsFolder,
				 Properties.Settings.Default.LoaderModuleLookupRetryDelay,  Properties.Settings.Default.ViewModelRefreshInterval, Properties.Settings.Default.EventsViewModelRefreshInterval, Properties.Settings.Default.MaxEventsViewModelChunkSize);

			InitializeComponent();
			DataContext = appViewModel;


			string[] args = Environment.GetCommandLineArgs();
			if (args!=null)
			{
				foreach (string arg in args.Skip(1))
				{
					if (arg.StartsWith("-"))
					{
						if (arg == "-SelfLogging") appViewModel.Open("LogInspect.log");
					}
					else
					{
						appViewModel.Open(arg);
					}
				}
			}
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			//Dispatcher.InvokeShutdown();
			appViewModel.Dispose();
			
		}

		private void ShowError(Exception ex)
		{
			ShowError(ex.Message);
		}
		private void ShowError(string Message)
		{
			MessageBox.Show(Message, "Unexpected error", MessageBoxButton.OK);
		}

		private void OpenCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;e.Handled = true;
		}

		private void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			OpenFileDialog dialog;

			dialog = new OpenFileDialog();
			dialog.Title = "Open log file";
			dialog.Filter = "Log files|*.log|Text files|*.txt|All files|*.*";

			if (dialog.ShowDialog(this)??false)
			{
				appViewModel.Open(dialog.FileName);
			}
		}

		#region Filter events
		private void FilterEventsCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem?.Status == Statuses.Idle) ; e.Handled = true;
		}

		private void FilterEventsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			appViewModel.SelectedItem.Refresh();
		}

		#endregion

		#region severities
		private void FindPreviousSeverityCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem!=null) && (appViewModel.SelectedItem.Status== Statuses.Idle)  ; e.Handled = true;
		}

		private async void FindPreviousSeverityCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			int index;
			index=await appViewModel.SelectedItem.FindPreviousSeverityAsync(appViewModel.SelectedItem.Severities.SelectedItem,appViewModel.SelectedItem.Events.SelectedItem?.EventIndex??-1);
			if (index >= 0) appViewModel.SelectedItem.Events.Select(index);
		}

		private void FindNextSeverityCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem != null) && (appViewModel.SelectedItem.Status == Statuses.Idle) ; e.Handled = true;
		}

		private async void FindNextSeverityCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			int index;
			index = await appViewModel.SelectedItem.FindNextSeverityAsync(appViewModel.SelectedItem.Severities.SelectedItem, appViewModel.SelectedItem.Events.SelectedItem?.EventIndex ?? -1);
			if (index >= 0) appViewModel.SelectedItem.Events.Select(index);
		}
		#endregion

		#region bookmarks
		private void ToogleBookMarkCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem != null) && (appViewModel.SelectedItem.Status == Statuses.Idle) && (appViewModel.SelectedItem.Events.SelectedItem != null); e.Handled = true;
		}

		private  void ToogleBookMarkCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			appViewModel.SelectedItem.ToogleBookMark();
		}
		private void FindPreviousBookMarkCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem != null) && (appViewModel.SelectedItem.Status == Statuses.Idle); e.Handled = true;
		}

		private async void FindPreviousBookMarkCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			int index;
			index = await appViewModel.SelectedItem.FindPreviousBookMarkAsync( appViewModel.SelectedItem.Events.SelectedItem?.EventIndex ?? -1);
			if (index >= 0) appViewModel.SelectedItem.Events.Select(index);
		}

		private void FindNextBookMarkCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem != null) && (appViewModel.SelectedItem.Status == Statuses.Idle) ; e.Handled = true;
		}

		private async void FindNextBookMarkCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			int index;
			index=await appViewModel.SelectedItem.FindNextBookMarkAsync( appViewModel.SelectedItem.Events.SelectedItem?.EventIndex ?? -1);
			if (index >= 0) appViewModel.SelectedItem.Events.Select(index);
		}




		#endregion

		#region timeline
		private void DecMinutesCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem != null) && (appViewModel.SelectedItem.Status == Statuses.Idle); e.Handled = true;
		}

		private async void DecMinutesCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			int index;
			index = await appViewModel.SelectedItem.DecMinutesAsync(appViewModel.SelectedItem.Events.SelectedItem?.EventIndex ?? -1);
			if (index >= 0) appViewModel.SelectedItem.Events.Select(index);
		}

		private void IncMinutesCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem != null) && (appViewModel.SelectedItem.Status == Statuses.Idle); e.Handled = true;
		}

		private async void IncMinutesCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			int index;
			index = await appViewModel.SelectedItem.IncMinutesAsync(appViewModel.SelectedItem.Events.SelectedItem?.EventIndex ?? -1);
			if (index >= 0) appViewModel.SelectedItem.Events.Select(index);
		}

		private void DecHoursCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem != null) && (appViewModel.SelectedItem.Status == Statuses.Idle); e.Handled = true;
		}

		private async void DecHoursCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			int index;
			index = await appViewModel.SelectedItem.DecHoursAsync(appViewModel.SelectedItem.Events.SelectedItem?.EventIndex ?? -1);
			if (index >= 0) appViewModel.SelectedItem.Events.Select(index);
		}

		private void IncHoursCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem != null) && (appViewModel.SelectedItem.Status == Statuses.Idle); e.Handled = true;
		}

		private async void IncHoursCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			int index;
			index = await appViewModel.SelectedItem.IncHoursAsync(appViewModel.SelectedItem.Events.SelectedItem?.EventIndex ?? -1);
			if (index >= 0) appViewModel.SelectedItem.Events.Select(index);
		}



		#endregion


		#region find
		private void FindCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem != null); e.Handled = true;
		}

		private void FindCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			appViewModel.SelectedItem.FindOptions.IsVisible = true;
		}
		private void FindPreviousCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem != null) && (appViewModel.SelectedItem.FindOptions.Column != null) && (appViewModel.SelectedItem.Status == Statuses.Idle) && (!string.IsNullOrEmpty(appViewModel.SelectedItem.FindOptions.Text)); e.Handled = true;
		}

		private async void FindPreviousCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			int index;

			index = await appViewModel.SelectedItem.FindPreviousAsync(appViewModel.SelectedItem.Events.SelectedItem?.EventIndex ?? -1, appViewModel.SelectedItem.FindOptions.Match);
			if (index >= 0) appViewModel.SelectedItem.Events.Select(index);
		}

		private void FindNextCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem != null) && (appViewModel.SelectedItem.FindOptions.Column != null) && (appViewModel.SelectedItem.Status == Statuses.Idle) && (!string.IsNullOrEmpty(appViewModel.SelectedItem.FindOptions.Text)); e.Handled = true;
		}

		private async void FindNextCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			int index;

			index = await appViewModel.SelectedItem.FindNextAsync(appViewModel.SelectedItem.Events.SelectedItem?.EventIndex ?? -1, appViewModel.SelectedItem.FindOptions.Match );
			if (index >= 0) appViewModel.SelectedItem.Events.Select(index);
		}
		#endregion


		private void CloseCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem != null) && (appViewModel.SelectedItem.Status == Statuses.Idle); e.Handled = true;

		}

		private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			appViewModel.CloseCurrent();
		}


	}
}
