using LogInspectLib;
using LogLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogInspect
{
	public class AppViewModel : ViewModel
	{
		private List<FormatHandler> formatHandlers;

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

		public AppViewModel(ILogger Logger,string Path) : base(Logger)
		{
			LogFiles = new ObservableCollection<LogFileViewModel>();
			formatHandlers = new List<FormatHandler>();
			LoadSchemas(Path);
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

			logFile = new LogFileViewModel(Logger, this,FileName);
			LogFiles.Add(logFile);
			SelectedItem = logFile;
		}

		public void LoadSchemas(string Path)
		{
			FormatHandler schema;

			Log(LogLevels.Information, "Parsing format handlers directory...");
			try
			{
				foreach (string FileName in Directory.EnumerateFiles(Path, "*.xml"))
				{
					Log(LogLevels.Information, $"Loading file {FileName}");
					try
					{
						schema = FormatHandler.LoadFromFile(FileName);
					}
					catch (Exception ex)
					{
						Log(ex);
						continue;
					}
					formatHandlers.Add(schema);
				}
			}
			catch (Exception ex)
			{
				Log(ex);
			}
		}

		public FormatHandler GetFormatHandler(string FileName)
		{
			FormatHandler formatHandler;
			string shortName;


			shortName = Path.GetFileName(FileName);
			Log(LogLevels.Information, $"Try to find a format handler for file {shortName}");
			formatHandler = formatHandlers.FirstOrDefault(item => item.MatchFileName(shortName));
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

		public LogReader CreateLogReader(string FileName)
		{
			Stream stream;
			LogReader reader;
			FormatHandler formatHandler;

			formatHandler = GetFormatHandler(FileName);

			Log(LogLevels.Information, "Creating log reader...");
			try
			{
				stream = new FileStream(FileName, FileMode.Open,FileAccess.Read);
				reader = new LogReader(stream, Encoding.Default, formatHandler);
			}
			catch (Exception ex)
			{
				Log(ex);
				return null;
			}

			return reader;
		}


	}
}
