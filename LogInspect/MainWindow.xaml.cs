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
			FormatHandler schema;
			Rule rule;
			Column column;

			#region rcm
			schema = new FormatHandler() { Name = "Nice.Perform.RCM", FileNamePattern = @"^RCM\.log(\.\d+)?$"};
			schema.AppendLineToNextPatterns.Add(@".*(?<!\u0003)$");

			rule = new LogInspectLib.Rule() { Name = "Event" };
			rule.Tokens.Add(new Token() { Name = "Date", Pattern = @"^\d\d/\d\d/\d\d \d\d:\d\d:\d\d\.\d+" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @" *\| *" });
			rule.Tokens.Add(new Token() { Name = "Severity", Pattern = @"[^ ]+" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @" *\| *" });
			rule.Tokens.Add(new Token() { Name = "Thread", Pattern = @"[^,]+(, *[^,]+)*" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @" *\| *" });
			rule.Tokens.Add(new Token() { Name = "Module", Pattern = @"[^ ]+" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @" *\| *" });
			rule.Tokens.Add(new Token() { Name = "Message", Pattern = @"[^\u0003]+" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @"\u0003$" });
			schema.Rules.Add(rule);

			rule = new LogInspectLib.Rule() { Name = "Comment" };
			rule.Tokens.Add(new Token() { Pattern = @"^//" });
			schema.Rules.Add(rule);
			
			schema.SaveToFile(System.IO.Path.Combine(Properties.Settings.Default.FormatHandlersFolder, "Nice.Perform.RCM.xml"));
			#endregion

			#region NTR
			schema = new FormatHandler() { Name = "Nice.NTR.Archiving", FileNamePattern = @"^CyberTech\.ContentManager\.Archiving\.WindowsService-\d\d\d\d-\d\d-\d\d\.log$" };
			schema.AppendLineToPreviousPatterns.Add(@"^[^\[]");
			schema.DiscardLinePatterns.Add(@"^$");
			schema.SeverityColumn = "Severity";
			schema.TimeStampColumn = "Date";
			schema.Columns.Add(new Column() { Name = "Date",  Width = 200, Alignment = "Center",Format = "yyyy-MM-dd HH:mm:ss.fff",Type="DateTime" });
			schema.Columns.Add(new Column() { Name = "Severity", Width = 100, Alignment = "Center", IsFilterItemSource = true });
			schema.Columns.Add(new Column() { Name = "Thread", Width = 300, IsFilterItemSource = true });
			column = new Column() { Name = "Message", Width = 600 };
			column.InlineColoringRules.Add(new LogInspectLib.InlineColoringRule() { Pattern = @"CVSKEY=\d+", Foreground = "Green",Bold=true });
			column.InlineColoringRules.Add(new LogInspectLib.InlineColoringRule() { Pattern = @"AudioFragment:\d+", Foreground = "Olive",Bold=true });
			column.InlineColoringRules.Add(new LogInspectLib.InlineColoringRule() { Pattern = @"([a-zA-Z]:\\|\\\\)[a-zA-Z0-9\.\-_]{1,}(\\[a-zA-Z0-9\-_()]{1,}){1,}[\$]{0,1}\.[\w]+", Foreground = "Blue",Underline=true });
			column.InlineColoringRules.Add(new LogInspectLib.InlineColoringRule() { Pattern = @"Error num=\d+", Foreground = "Red", Bold = true });
			column.InlineColoringRules.Add(new LogInspectLib.InlineColoringRule() { Pattern = @"Error = [^\.]+", Foreground = "Red", Bold = true });
			column.InlineColoringRules.Add(new LogInspectLib.InlineColoringRule() { Pattern = @"'[^']+'", Foreground = "BlueViolet" });
			column.InlineColoringRules.Add(new LogInspectLib.InlineColoringRule() { Pattern = @"\d+(\.\d+)? ms", Foreground = "Black", Bold = true });
			column.InlineColoringRules.Add(new LogInspectLib.InlineColoringRule() { Pattern = @" \d+ ", Foreground = "Black", Bold = true });
			column.InlineColoringRules.Add(new LogInspectLib.InlineColoringRule() { Pattern = @"Connected|CorrectMediumFound", Foreground = "RosyBrown",  Italic = true });
			

			schema.Columns.Add(column);

			rule = new LogInspectLib.Rule() { Name = "Event with thread" };
			rule.Tokens.Add(new Token() { Name = null, Pattern = @"^\[" });
			rule.Tokens.Add(new Token() { Name = "Date", Pattern = @"\d\d\d\d-\d\d-\d\d \d\d:\d\d:\d\d\.\d+" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @" " });
			rule.Tokens.Add(new Token() { Name = "Severity", Pattern = @"[^\]]+" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @"] " });
			rule.Tokens.Add(new Token() { Name = "Thread", Pattern = @"[^:]+"});
			rule.Tokens.Add(new Token() { Name = null, Pattern = @": " });
			rule.Tokens.Add(new Token() { Name = "Message", Pattern = @".+"});
			schema.Rules.Add(rule);

			rule = new LogInspectLib.Rule() { Name = "Event without thread" };
			rule.Tokens.Add(new Token() { Name = null, Pattern = @"^\[" });
			rule.Tokens.Add(new Token() { Name = "Date", Pattern = @"\d\d\d\d-\d\d-\d\d \d\d:\d\d:\d\d\.\d+" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @" " });
			rule.Tokens.Add(new Token() { Name = "Severity", Pattern = @"[^\]]+" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @"] " });
			rule.Tokens.Add(new Token() { Name = "Message", Pattern = @".+" });
			schema.Rules.Add(rule);

			schema.EventColoringRules.Add(new EventColoringRule() { Column = "Severity", Pattern = @"ERROR", Background="OrangeRed" });
			schema.EventColoringRules.Add(new EventColoringRule() { Column = "Severity", Pattern = @"WARN", Background="Orange" });

			schema.SaveToFile(System.IO.Path.Combine(Properties.Settings.Default.FormatHandlersFolder, "Nice.NTR.Archiving.xml"));
			#endregion


			logger = new ConsoleLogger(new DefaultLogFormatter());
			appViewModel = new AppViewModel(logger, Properties.Settings.Default.FormatHandlersFolder, Properties.Settings.Default.BufferSize,
				 Properties.Settings.Default.IndexerLookupRetryDelay, Properties.Settings.Default.IndexerBufferLookupRetryDelay, Properties.Settings.Default.IndexerProgressRefreshDelay);

			InitializeComponent();
			DataContext = appViewModel;
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
		private void SetBookMarkCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
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
