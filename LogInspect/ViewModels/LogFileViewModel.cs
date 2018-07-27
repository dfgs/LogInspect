using LogInspect.Modules;
using LogInspectLib;
using LogInspectLib.Readers;
using LogLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace LogInspect.ViewModels
{
	public class LogFileViewModel:ViewModel,IEnumerable<Event>,INotifyCollectionChanged
	{

		public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(int), typeof(LogFileViewModel), new PropertyMetadata(-1, PositionPropertyChanged));
		public int Position
		{
			get { return (int)GetValue(PositionProperty); }
			set { SetValue(PositionProperty, value); }
		}

		public static readonly DependencyProperty PageSizeProperty = DependencyProperty.Register("PageSize", typeof(int), typeof(LogFileViewModel), new PropertyMetadata(0, PageSizePropertyChanged));
		public int PageSize
		{
			get { return (int)GetValue(PageSizeProperty); }
			set { SetValue(PageSizeProperty, value); }
		}


		public int Count
		{
			get;
			private set;
		}

		public string FileName
		{
			get;
			private set;
		}
		public string Name
		{
			get;
			private set;
		}

		

		public ColumnViewModel[] Columns
		{
			get;
			private set;
		}

		private Event[] items;
		private AppViewModel appViewModel;
		private EventIndexerModule eventIndexerModule;
		private EventReader eventReader;

		private bool isDisposing;   // prevent hangs when closing application

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public LogFileViewModel(ILogger Logger, AppViewModel AppViewModel, string FileName,int BufferSize):base(Logger)
		{
			EventReader indexerReader;

			isDisposing = false;
			this.FileName = FileName;
			this.Name = Path.GetFileName(FileName);
			this.appViewModel = AppViewModel;

			this.items = new Event[PageSize];

			this.eventReader = AppViewModel.CreateEventReader(FileName,BufferSize);
			
			if (this.eventReader.FormatHandler.Rules.Count == 0) Columns = new ColumnViewModel[0];
			else Columns=this.eventReader.FormatHandler.Rules.First().GetColumns().Select(item=> new ColumnViewModel(Logger,item,150)).ToArray();

			indexerReader = AppViewModel.CreateEventReader(FileName,BufferSize);
			if (indexerReader != null)
			{
				eventIndexerModule = new EventIndexerModule(Logger, indexerReader);
				eventIndexerModule.EventIndexed += EventIndexerModule_EventIndexed;
				Log(LogLevels.Debug, "Starting EventIndexer");
				eventIndexerModule.Start();
			}
			else eventIndexerModule = null;

			Position = 0;
		}


		public override void Dispose()
		{
			isDisposing = true;
			if (eventIndexerModule != null)
			{
				Log(LogLevels.Debug, "Stopping EventIndexer");
				eventIndexerModule.Stop();
			}
			
		}

		protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			CollectionChanged?.Invoke(this, e);
		}

		private static void PageSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((LogFileViewModel)d).OnPageSizeChanged((int)e.OldValue, (int)e.NewValue);
		}
		protected virtual void OnPageSizeChanged(int OldSize, int NewSize)
		{
			items = new Event[NewSize];
			LoadItems(Position);
		}

		private static void PositionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((LogFileViewModel)d).OnPositionChanged((int)e.OldValue, (int)e.NewValue);
		}
		protected virtual void OnPositionChanged(int OldPosition,int NewPosition)
		{
			LoadItems(NewPosition);
		}

		private void LoadItems(int Position)
		{
			long pos;
			Event ev;


			pos = eventIndexerModule?.GetStreamPos(Position) ?? -1;
			if (pos == -1)
			{
				Log(LogLevels.Error, $"Failed to seek to position {Position}");
				return;
			}
			eventReader.Seek(pos);
			for (int t = 0; t < PageSize; t++)
			{
				try
				{
					ev = eventReader.Read();
				}
				catch (Exception ex)
				{
					Log(ex);
					return;
				}
				items[t] = ev;
			}
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		private void EventIndexerModule_EventIndexed(Event Ev, int Index)
		{
			if (!isDisposing)	// prevent hangs when closing application
			{
				Dispatcher.Invoke(() =>
				{
					Count=eventIndexerModule.Count;
					OnPropertyChanged("Count");
					//OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
					//CollectionViewSource.GetDefaultView(this).Refresh();
				});
			}
		}

		public IEnumerator<Event> GetEnumerator()
		{
			foreach (Event item in items) yield return item;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
