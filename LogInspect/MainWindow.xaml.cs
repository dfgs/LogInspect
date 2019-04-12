using LogInspect.BaseLib;
using LogInspect.BaseLib.Builders;
using LogInspect.BaseLib.FileLoaders;
using LogInspect.BaseLib.Parsers;
using LogInspect.Models;
using LogInspect.Modules;
using LogInspect.ViewModels;
using LogInspect.ViewModels.Columns;
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
		private IPatternLibraryModule patternLibraryModule;
		private IInlineFormatLibraryModule inlineColoringRuleLibraryModule;
		private IFormatHandlerLibraryModule formatHandlerLibraryModule;

		private AppViewModel appViewModel;

		public MainWindow()
		{

			logger = new FileLogger(new DefaultLogFormatter(),"LogInspect.log");
			//logger = new ConsoleLogger(new DefaultLogFormatter());

			appViewModel = new AppViewModel(logger);

			patternLibraryModule = new PatternLibraryModule(logger,new DirectoryEnumerator(),new PatternLibLoader());
			inlineColoringRuleLibraryModule = new InlineFormatLibraryModule(logger, new DirectoryEnumerator(),new InlineColoringRuleLibLoader());
			formatHandlerLibraryModule = new FormatHandlerLibraryModule(logger, new DirectoryEnumerator(), new FormatHandlerLoader(),patternLibraryModule);

			patternLibraryModule.LoadDirectory(Properties.Settings.Default.PatternsFolder);
			inlineColoringRuleLibraryModule.LoadDirectory(Properties.Settings.Default.InlineFormatsFolder);
			formatHandlerLibraryModule.LoadDirectory(Properties.Settings.Default.FormatHandlersFolder);
			
			InitializeComponent();
			DataContext = appViewModel;

			//-NoSelfLogging "E:\FTP\BNP\8202573775\RuleEngine.txt.3"
			string[] args = Environment.GetCommandLineArgs();
			if (args!=null)
			{
				foreach (string arg in args.Skip(1))
				{
					if (arg.StartsWith("-"))
					{
						if (arg == "-SelfLogging") Open("LogInspect.log").Wait();
					}
					else
					{
						Open(arg).Wait();
					}
				}
			}
		}

		

		private void ShowError(Exception ex)
		{
			ShowError(ex.Message);
		}
		private void ShowError(string Message)
		{
			MessageBox.Show(Message, "Unexpected error", MessageBoxButton.OK);
		}
		private void ShowNotFound()
		{
			MessageBox.Show("No more item found", "Not found", MessageBoxButton.OK);
		}
		private void OpenCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;e.Handled = true;
		}

		private async void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			OpenFileDialog dialog;

			dialog = new OpenFileDialog();
			dialog.Title = "Open log file";
			dialog.Filter = "Log files|*.log|Text files|*.txt|All files|*.*";

			if (dialog.ShowDialog(this) ?? false) await Open(dialog.FileName);
		}

		private async Task Open(string FileName)
		{
			FormatHandlerSelectionWindow window;

			LogFile logFile;
			ILogFileLoaderModule logFileLoaderModule;
			LoadWindow loadWindow;
			IColorProviderModule colorProviderModule;
			IInlineParserFactoryModule inlineParserBuilderModule;
			ILineReader lineReader;
			ILineBuilder lineBuilder;
			ILogBuilder logBuilder;
			LogParser logParser;
			IStringMatcherFactoryModule stringMatcherFactoryModule;
			FileStream stream;
			IProgressReporter progressReporter;
			FormatHandler[] formatHandlers;
			FormatHandler formatHandler;

			formatHandlers = formatHandlerLibraryModule.GetFormatHandlers(FileName).ToArray();

			if (formatHandlers.Length==0)
			{
				ShowError("Cannot find any format handler");
				return;
			} else if (formatHandlers.Length<3)
			{
				formatHandler = formatHandlers[0];
			}
			else
			{
				window = new FormatHandlerSelectionWindow() { Owner=this,FormatHandlers=formatHandlers.Take(formatHandlers.Length-1)};
				if (!window.ShowDialog() ?? false) return;
				if (window.SelectedFormatHandler == null) return;
				formatHandler = window.SelectedFormatHandler;
			}

			logFile = new LogFile(FileName, formatHandler) ;
			colorProviderModule = new ColorProviderModule(logger,logFile.FormatHandler.EventColoringRules);
			inlineParserBuilderModule = new InlineParserFactoryModule(logger,patternLibraryModule,inlineColoringRuleLibraryModule);

			stringMatcherFactoryModule = new StringMatcherFactoryModule(logger, patternLibraryModule);

			lineBuilder = new LineBuilder(stringMatcherFactoryModule.CreateStringMatcher(logFile.FormatHandler.NameSpace, logFile.FormatHandler.DiscardLinePatterns));
			logBuilder = new LogBuilder(
				stringMatcherFactoryModule.CreateStringMatcher(logFile.FormatHandler.NameSpace, logFile.FormatHandler.Rules.Where(item => item.Discard).Select(item => item.GetPattern())),
				stringMatcherFactoryModule.CreateStringMatcher(logFile.FormatHandler.NameSpace, logFile.FormatHandler.AppendLineToPreviousPatterns),
				stringMatcherFactoryModule.CreateStringMatcher(logFile.FormatHandler.NameSpace, logFile.FormatHandler.AppendLineToNextPatterns)
				);
			logParser = new LogParser(logFile.FormatHandler.Columns);
			logParser.Add(patternLibraryModule.Build(logFile.FormatHandler.NameSpace, logFile.FormatHandler.Rules.Select(item=>item.GetPattern()), true));
			
			try
			{
				stream = new FileStream(FileName, FileMode.Open,FileAccess.Read,FileShare.ReadWrite);
			}
			catch(Exception ex)
			{
				ShowError(ex);
				return;
			}

			using (stream)
			{
				progressReporter = new StreamProgressReporter(stream);
				lineReader = new FileLineReader(stream);
				logFileLoaderModule = new LogFileLoaderModule(logger, lineReader, lineBuilder, logBuilder, logParser);
				//logFileLoaderModule = new InfiniteLogFileLoaderModule(logger);

				loadWindow = new LoadWindow(logFileLoaderModule,progressReporter, logFile); loadWindow.Owner = this;
				if (loadWindow.Load() ?? false)
				{
					try
					{
						await appViewModel.Open(logFile, inlineParserBuilderModule, colorProviderModule);
					}
					catch(Exception ex)
					{
						ShowError(ex);
					}
				}
			}

		}

		#region Filter events
		private void FilterEventsCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem?.Status == Statuses.Idle) ; e.Handled = true;
		}

		private async void FilterEventsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			await appViewModel.SelectedItem.Refresh();
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
			index=await appViewModel.SelectedItem.FindPreviousSeverityAsync(appViewModel.SelectedItem.Severities.SelectedItem,appViewModel.SelectedItem.FilteredEvents.SelectedIndex);
			if (index >= 0) appViewModel.SelectedItem.FilteredEvents.Select(index);
			else ShowNotFound();
		}

		private void FindNextSeverityCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem != null) && (appViewModel.SelectedItem.Status == Statuses.Idle) ; e.Handled = true;
		}

		private async void FindNextSeverityCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			int index;
			index = await appViewModel.SelectedItem.FindNextSeverityAsync(appViewModel.SelectedItem.Severities.SelectedItem, appViewModel.SelectedItem.FilteredEvents.SelectedIndex);
			if (index >= 0) appViewModel.SelectedItem.FilteredEvents.Select(index);
			else ShowNotFound();
		}
		#endregion

		#region bookmarks
		private void ToogleBookMarkCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem != null) && (appViewModel.SelectedItem.Status == Statuses.Idle) && (appViewModel.SelectedItem.FilteredEvents.SelectedItem != null); e.Handled = true;
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
			index = await appViewModel.SelectedItem.FindPreviousBookMarkAsync( appViewModel.SelectedItem.FilteredEvents.SelectedIndex);
			if (index >= 0) appViewModel.SelectedItem.FilteredEvents.Select(index);
			else ShowNotFound();
		}

		private void FindNextBookMarkCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem != null) && (appViewModel.SelectedItem.Status == Statuses.Idle) ; e.Handled = true;
		}

		private async void FindNextBookMarkCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			int index;
			index=await appViewModel.SelectedItem.FindNextBookMarkAsync( appViewModel.SelectedItem.FilteredEvents.SelectedIndex);
			if (index >= 0) appViewModel.SelectedItem.FilteredEvents.Select(index);
			else ShowNotFound();
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
			index = await appViewModel.SelectedItem.DecMinutesAsync(appViewModel.SelectedItem.FilteredEvents.SelectedIndex);
			if (index >= 0) appViewModel.SelectedItem.FilteredEvents.Select(index);
			else ShowNotFound();
		}

		private void IncMinutesCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem != null) && (appViewModel.SelectedItem.Status == Statuses.Idle); e.Handled = true;
		}

		private async void IncMinutesCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			int index;
			index = await appViewModel.SelectedItem.IncMinutesAsync(appViewModel.SelectedItem.FilteredEvents.SelectedIndex);
			if (index >= 0) appViewModel.SelectedItem.FilteredEvents.Select(index);
			else ShowNotFound();
		}

		private void DecHoursCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem != null) && (appViewModel.SelectedItem.Status == Statuses.Idle); e.Handled = true;
		}

		private async void DecHoursCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			int index;
			index = await appViewModel.SelectedItem.DecHoursAsync(appViewModel.SelectedItem.FilteredEvents.SelectedIndex);
			if (index >= 0) appViewModel.SelectedItem.FilteredEvents.Select(index);
			else ShowNotFound();
		}

		private void IncHoursCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem != null) && (appViewModel.SelectedItem.Status == Statuses.Idle); e.Handled = true;
		}

		private async void IncHoursCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			int index;
			index = await appViewModel.SelectedItem.IncHoursAsync(appViewModel.SelectedItem.FilteredEvents.SelectedIndex);
			if (index >= 0) appViewModel.SelectedItem.FilteredEvents.Select(index);
			else ShowNotFound();
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

			index = await appViewModel.SelectedItem.FindPreviousAsync(appViewModel.SelectedItem.FilteredEvents.SelectedIndex, appViewModel.SelectedItem.FindOptions.Match);
			if (index >= 0) appViewModel.SelectedItem.FilteredEvents.Select(index);
			else ShowNotFound();
		}

		private void FindNextCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem != null) && (appViewModel.SelectedItem.FindOptions.Column != null) && (appViewModel.SelectedItem.Status == Statuses.Idle) && (!string.IsNullOrEmpty(appViewModel.SelectedItem.FindOptions.Text)); e.Handled = true;
		}

		private async void FindNextCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			int index;

			index = await appViewModel.SelectedItem.FindNextAsync(appViewModel.SelectedItem.FilteredEvents.SelectedIndex, appViewModel.SelectedItem.FindOptions.Match );
			if (index >= 0) appViewModel.SelectedItem.FilteredEvents.Select(index);
			else ShowNotFound();
		}
		#endregion

		private void EditColumnsFormatCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem != null) && (appViewModel.SelectedItem.Status == Statuses.Idle); e.Handled = true;
		}

		private async void EditColumnsFormatCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ColumnsFormatWindow window;

			window = new ColumnsFormatWindow(); window.Owner = Application.Current.MainWindow; window.ColumnFormatViewModels = appViewModel.SelectedItem.Columns.Select(item=>item.CreateColumnFormatViewModel() ).ToArray() ;
			if (window.ShowDialog() ?? false)
			{
				foreach (ColumnFormatViewModel vm in window.ColumnFormatViewModels)
				{
					vm.ApplyNewFormat();
				}
				await appViewModel.SelectedItem.ReloadEvents();
			}

		}
		private void CloseCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (appViewModel.SelectedItem != null) && (appViewModel.SelectedItem.Status == Statuses.Idle); e.Handled = true;

		}

		private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			LogFileViewModel logFile;

			logFile = e.Parameter as LogFileViewModel;
			if (logFile!=null) appViewModel.Close(logFile);
		}


	}
}
