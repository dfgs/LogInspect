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
	public class LogFileViewModel:ViewModel,IEnumerable<EventViewModel>,INotifyCollectionChanged
	{

		public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(int), typeof(LogFileViewModel), new PropertyMetadata(0, PositionPropertyChanged,PositionPropertyCoerced));
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

		private List<EventViewModel> items;
		private EventIndexerModule eventIndexerModule;
		private EventReader pageEventReader;

		private bool isDisposing;   // prevent hangs when closing application

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public LogFileViewModel(ILogger Logger, EventReader PageEventReader,EventReader IndexerEventReader ):base(Logger)
		{

			isDisposing = false;
			this.FileName = FileName;
			this.Name = Path.GetFileName(FileName);

			this.items = new List<EventViewModel>();
			this.pageEventReader = PageEventReader;
			
			if (this.pageEventReader.FormatHandler.Rules.Count == 0) Columns = new ColumnViewModel[0];
			else Columns=this.pageEventReader.FormatHandler.Rules.First().GetColumns().Select(item=> new ColumnViewModel(Logger,item,150)).ToArray();

			eventIndexerModule = new EventIndexerModule(Logger, IndexerEventReader);
			eventIndexerModule.Updated += EventIndexerModule_Updated;

			LoadItems(0);

			Log(LogLevels.Debug, "Starting EventIndexer");
			eventIndexerModule.Start();
			
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
			LoadItems(Position);
		}

		private static object PositionPropertyCoerced(DependencyObject d, object baseValue)
		{
			int value;
			LogFileViewModel vm;

			vm = (LogFileViewModel)d;
			value = (int)baseValue;
			if ((value < 0) || (value>=vm.Count)) return DependencyProperty.UnsetValue;
			return baseValue;
		}

		private static void PositionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((LogFileViewModel)d).OnPositionChanged((int)e.OldValue, (int)e.NewValue);
		}
		protected virtual void OnPositionChanged(int OldPosition,int NewPosition)
		{
			LoadItems(NewPosition);
		}

		private void LoadItems(int Index)
		{
			long pos;
			Event ev;

			//if (Position == -1) return;

			items.Clear();

			pos = eventIndexerModule?.GetStreamPos(Index) ?? -1;
			if (pos == -1)
			{
				Log(LogLevels.Error, $"Failed to seek to position {Index}");
				return;
			}
			pageEventReader.Seek(pos);
			for (int t = 0; (t < PageSize) && (!pageEventReader.EndOfStream); t++)
			{
				try
				{
					ev = pageEventReader.Read();
				}
				catch (Exception ex)
				{
					Log(ex);
					return;
				}
				items.Add(new EventViewModel(Logger,ev,Index+t));
			}
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		private void EventIndexerModule_Updated(object sender,EventArgs e)
		{
			if (!isDisposing)	// prevent hangs when closing application
			{
				Dispatcher.Invoke(() =>
				{
					Count=eventIndexerModule.Count;
					OnPropertyChanged("Count");
				});
			}
		}

		public IEnumerator<EventViewModel> GetEnumerator()
		{
			return items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
