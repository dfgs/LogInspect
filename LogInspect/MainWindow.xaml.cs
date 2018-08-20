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

			#region rcm
			schema = new FormatHandler();
			schema.Name = "Nice.Perform.RCM";
			schema.FileNamePattern = @"^RCM\.log(\.\d+)?$";
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
			schema = new FormatHandler();
			schema.Name = "Nice.NTR.Archiving";
			schema.FileNamePattern = @"^CyberTech\.ContentManager\.Archiving\.WindowsService-\d\d\d\d-\d\d-\d\d\.log$";
			schema.AppendLineToPreviousPatterns.Add(@"^[^\[]");
			schema.DiscardLinePatterns.Add(@"^$");
			//[2018-07-07 00:00:01.637 START] Starting new log-file or starting application.

			rule = new LogInspectLib.Rule() { Name = "Event with thread" };
			rule.SeverityToken = "Severity";
			rule.Tokens.Add(new Token() { Name = null, Pattern = @"^\[" });
			rule.Tokens.Add(new Token() { Name = "Date", Pattern = @"\d\d\d\d-\d\d-\d\d \d\d:\d\d:\d\d\.\d+",Width=200,Alignment="Center" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @" " });
			rule.Tokens.Add(new Token() { Name = "Severity", Pattern = @"[^\]]+", Width = 100, Alignment = "Center" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @"] " });
			rule.Tokens.Add(new Token() { Name = "Thread", Pattern = @"[^:]+", Width = 300 });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @": " });
			rule.Tokens.Add(new Token() { Name = "Message", Pattern = @".+", Width = 600 });
			schema.Rules.Add(rule);

			rule = new LogInspectLib.Rule() { Name = "Event without thread" };
			rule.SeverityToken = "Severity";
			rule.Tokens.Add(new Token() { Name = null, Pattern = @"^\[" });
			rule.Tokens.Add(new Token() { Name = "Date", Pattern = @"\d\d\d\d-\d\d-\d\d \d\d:\d\d:\d\d\.\d+", Alignment = "Center" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @" " });
			rule.Tokens.Add(new Token() { Name = "Severity", Pattern = @"[^\]]+", Alignment = "Center" });
			rule.Tokens.Add(new Token() { Name = null, Pattern = @"] " });
			rule.Tokens.Add(new Token() { Name = "Message", Pattern = @".+" });
			schema.Rules.Add(rule);

			schema.SeverityMapping.Add(new SeverityMapping() {  Pattern = @"INFO", Severity = "Info" });
			schema.SeverityMapping.Add(new SeverityMapping() {  Pattern = @"ERROR", Severity = "Error" });
			schema.SeverityMapping.Add(new SeverityMapping() {  Pattern = @"WARN", Severity = "Warning" });

			schema.SaveToFile(System.IO.Path.Combine(Properties.Settings.Default.FormatHandlersFolder, "Nice.NTR.Archiving.xml"));
			#endregion


			logger = new ConsoleLogger(new DefaultLogFormatter());
			appViewModel = new AppViewModel(logger, Properties.Settings.Default.FormatHandlersFolder, Properties.Settings.Default.BufferSize, Properties.Settings.Default.PageSize, Properties.Settings.Default.PageCount, Properties.Settings.Default.IndexerLookupRetryDelay, Properties.Settings.Default.FiltererLookupRetryDelay);

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
		private async void FilterEventsCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem?.IsWorking == false) ; e.Handled = true;
			await Task.Yield();
		}

		private async void FilterEventsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			await appViewModel.SelectedItem.FilterEventsAsync();
		}

		#endregion

		#region severities
		private void FindPreviousSeverityCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem?.IsWorking==false) ; e.Handled = true;
		}

		private async void FindPreviousSeverityCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			await appViewModel.SelectedItem.FindPreviousAsync(appViewModel.SelectedItem.Severities.SelectedItem.Name);
		}

		private void FindNextSeverityCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem?.IsWorking == false); e.Handled = true;
		}

		private async void FindNextSeverityCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			await appViewModel.SelectedItem.FindNextAsync(appViewModel.SelectedItem.Severities.SelectedItem.Name);
		}
		#endregion

		#region bookmarks
		private void SetBookMarkCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem?.IsWorking == false) && (appViewModel.SelectedItem.SelectedItem!=null); e.Handled = true;
		}

		private async void ToogleBookMarkCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			await appViewModel.SelectedItem.ToogleBookMarkAsync();
		}
		private void FindPreviousBookMarkCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem?.IsWorking == false) ; e.Handled = true;
		}

		private async void FindPreviousBookMarkCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			await appViewModel.SelectedItem.FindPreviousBookMarkAsync(appViewModel.SelectedItem.Severities.SelectedItem.Name);
		}

		private void FindNextBookMarkCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem?.IsWorking == false) ; e.Handled = true;
		}

		private async void FindNextBookMarkCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			await appViewModel.SelectedItem.FindNextBookMarkAsync(appViewModel.SelectedItem.Severities.SelectedItem.Name);
		}
		#endregion





	}
}
