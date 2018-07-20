using LogInspectLib;
using LogLib;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
			InitializeComponent();


			FormatHandler schema = new FormatHandler();
			schema.Name = "RCM";
			schema.FileNamePattern = @"^RCM\.log(\.\d+)?$";
			schema.AppendToNextPatterns.Add(@".*(?<!\u0003)$");
			

			Rule rule = new LogInspectLib.Rule() { Name = "Event" };
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
			
			schema.SaveToFile(System.IO.Path.Combine(Properties.Settings.Default.FormatHandlersFolder, "RCM.xml"));

			logger = new ConsoleLogger(new DefaultLogFormatter());

			appViewModel = new AppViewModel(logger,Properties.Settings.Default.FormatHandlersFolder);
			DataContext = appViewModel;
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
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
				appViewModel.Open(dialog.FileName,Properties.Settings.Default.BufferSize);
			}
		}


		


	}
}
