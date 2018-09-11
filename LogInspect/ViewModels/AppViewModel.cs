﻿using LogInspectLib;
using LogInspectLib.Readers;
using LogLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogInspect.ViewModels
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

		private int bufferSize;
		private int indexerLookupRetryDelay;
		private int indexerBufferLookupRetryDelay;
		private int indexerProgressRefreshDelay;

		public AppViewModel(ILogger Logger,string Path, int BufferSize,int IndexerLookupRetryDelay, int IndexerBufferLookupRetryDelay,int IndexerProgressRefreshDelay) : base(Logger)
		{
			this.bufferSize = BufferSize;
			this.indexerLookupRetryDelay = IndexerLookupRetryDelay;
			this.indexerBufferLookupRetryDelay = IndexerBufferLookupRetryDelay;
			this.indexerProgressRefreshDelay = IndexerProgressRefreshDelay;

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
			EventReader indexerEventReader;

			indexerEventReader = CreateEventReader(FileName, bufferSize);
			if (indexerEventReader == null) return;

			try
			{
				logFile = new LogFileViewModel(Logger,FileName, indexerEventReader,indexerLookupRetryDelay,indexerBufferLookupRetryDelay, indexerProgressRefreshDelay);
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


		public void LoadSchemas(string Path)
		{
			FormatHandler schema;

			Log(LogLevels.Information, "Parsing format handlers directory...");
			try
			{
				foreach (string FileName in Directory.EnumerateFiles(Path, "*.xml").OrderBy((item)=>item) )
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

		public EventReader CreateEventReader(string FileName,int BufferSize)
		{
			Stream stream;
			EventReader reader;
			FormatHandler formatHandler;

			formatHandler = GetFormatHandler(FileName);

			Log(LogLevels.Information, "Creating event reader...");
			try
			{
				stream = new FileStream(FileName, FileMode.Open,FileAccess.Read,FileShare.ReadWrite);
				reader = new EventReader(stream, Encoding.Default,BufferSize, formatHandler);
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
