using LogInspectLib;
using LogInspectLib.Loaders;
using LogInspectLib.Parsers;
using LogLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace LogInspect.ViewModels
{
	public class AppViewModel : ViewModel
	{
		private List<FormatHandler> formatHandlers;
		private IRegexBuilder regexBuilder;

		public ObservableCollection<LogFileViewModel> LogFiles
		{
			get;
			private set;
		}
		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(LogFileViewModel), typeof(AppViewModel));
		public LogFileViewModel SelectedItem
		{
			get { return (LogFileViewModel)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		private int loaderModuleLookupRetryDelay;
		private int viewModelRefreshDelay;

		public AppViewModel(ILogger Logger,string FormatHandlersPath,string PatternLibsPath, int LoaderModuleLookupRetryDelay, int ViewModelRefreshDelay) : base(Logger,-1)
		{
			this.loaderModuleLookupRetryDelay = LoaderModuleLookupRetryDelay;
			this.viewModelRefreshDelay = ViewModelRefreshDelay;

			this.regexBuilder = new RegexBuilder();
			LoadPatternLibs(PatternLibsPath);

			LogFiles = new ObservableCollection<LogFileViewModel>();
			formatHandlers = new List<FormatHandler>();
			LoadSchemas(FormatHandlersPath);
		}

		public override void Dispose()
		{
			foreach(LogFileViewModel logFile in LogFiles)
			{
				logFile.Dispose();
			}
		}

		public void Open(string FileName)
		{
			LogFileViewModel logFile;
			FormatHandler formatHandler;

			formatHandler = GetFormatHandler(FileName);

			try
			{
				logFile = new LogFileViewModel(Logger,FileName, formatHandler,regexBuilder,loaderModuleLookupRetryDelay,viewModelRefreshDelay);
			}
			catch(Exception ex)
			{
				Log(ex);
				return;
			}
			LogFiles.Add(logFile);
			SelectedItem = logFile;
		}

		public void CloseCurrent()
		{
			if (SelectedItem == null) return;
			SelectedItem.Dispose();
			LogFiles.Remove(SelectedItem);
			SelectedItem = LogFiles.FirstOrDefault();
		}


		public void LoadPatternLibs(string Path)
		{
			PatternLib lib;

			Log(LogLevels.Information, "Parsing pattern libs directory...");
			try
			{
				foreach (string FileName in Directory.EnumerateFiles(Path, "*.xml").OrderBy((item) => item))
				{
					Log(LogLevels.Information, $"Loading file {FileName}");
					try
					{
						lib = PatternLib.LoadFromFile(FileName);
					}
					catch (Exception ex)
					{
						Log(ex);
						continue;
					}
					regexBuilder.Add(lib.NameSpace,lib);
				}
			}
			catch (Exception ex)
			{
				Log(ex);
			}
		}
		public void LoadSchemas(string Path)
		{
			FormatHandler formatHandler;

			Log(LogLevels.Information, "Parsing format handlers directory...");
			try
			{
				foreach (string FileName in Directory.EnumerateFiles(Path, "*.xml").OrderBy((item)=>item) )
				{
					Log(LogLevels.Information, $"Loading file {FileName}");
					try
					{
						formatHandler = FormatHandler.LoadFromFile(FileName);
					}
					catch (Exception ex)
					{
						Log(ex);
						continue;
					}
					formatHandlers.Add(formatHandler);
				}
			}
			catch (Exception ex)
			{
				Log(ex);
			}
		}

		private bool MatchFileName(string FileName, string FileNamePattern,string DefaultNameSpace)
		{
			Regex regex;

			try
			{
				regex = regexBuilder.Build(DefaultNameSpace, FileNamePattern);
				return regex.Match(FileName).Success;
			}
			catch(Exception ex)
			{
				Log(ex);
				return false;
			}
		}

		public FormatHandler GetFormatHandler(string FileName)
		{
			FormatHandler formatHandler;
			string shortName;


			shortName = Path.GetFileName(FileName);
			Log(LogLevels.Information, $"Try to find a format handler for file {shortName}");
			formatHandler = formatHandlers.FirstOrDefault(item => MatchFileName(shortName,item.FileNamePattern,item.DefaultNameSpace) );
			if (formatHandler == null)
			{
				Log(LogLevels.Warning, $"Format of log file {shortName} is unmanaged");
				formatHandler = new FormatHandler();    // create a default handler
			}
			else
			{
				Log(LogLevels.Information, $"Format handler {formatHandler.Name} found for log file {shortName}");
			}

			return formatHandler;
		}

		
		



	}
}
