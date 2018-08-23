using LogLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogInspect.ViewModels
{
	public class EventsViewModel:ViewModel,IEnumerable<EventViewModel>,INotifyCollectionChanged
	{
		private List<EventViewModel> items;


		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(EventViewModel), typeof(EventsViewModel));
		public EventViewModel SelectedItem
		{
			get { return (EventViewModel)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}


		public static readonly DependencyProperty MaxItemsProperty = DependencyProperty.Register("MaxItems", typeof(int), typeof(EventsViewModel));
		public int MaxItems
		{
			get { return (int)GetValue(MaxItemsProperty); }
			set { SetValue(MaxItemsProperty, value); }
		}


		public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(int), typeof(EventsViewModel),new PropertyMetadata(PositionPropertyChanged));
		public int Position
		{
			get { return (int)GetValue(PositionProperty); }
			set { SetValue(PositionProperty, value); }
		}

		private LogFileViewModel logFileViewModel;
		private int FirstItemIndex;

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public EventsViewModel(ILogger Logger ,LogFileViewModel LogFileViewModel) : base(Logger)
		{
			items = new List<EventViewModel>();
			this.logFileViewModel = LogFileViewModel;
		}


		protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			Dispatcher.Invoke(() => CollectionChanged?.Invoke(this, e));

		}

		public IEnumerator<EventViewModel> GetEnumerator()
		{
			return items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		private void RemoveInternal(int Index)
		{
			EventViewModel item;

			item = items[Index];
			items.RemoveAt(Index);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, Index));
		}
		private void AddInternal(EventViewModel Item)
		{
			int index;

			index = items.Count;
			items.Add(Item);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Item, index));
		}



		private static async void PositionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			await ((EventsViewModel)d).UpdateItemsAsync((int)e.OldValue, (int)e.NewValue);
		}


		private async Task UpdateItemsAsync(int OldPosition,int NewPosition)
		{
			int itemIndex, childIndex;
			int delta;
			int maxItemIndex;
			EventViewModel item;

			itemIndex = Position;

			maxItemIndex = itemIndex + MaxItems - 1;
			if (maxItemIndex >= logFileViewModel.EventIndexer.IndexedEventsCount) maxItemIndex = logFileViewModel.EventIndexer.IndexedEventsCount - 1;
			
			if (itemIndex >= FirstItemIndex)
			{
				// remove children that are above top
				delta = itemIndex - FirstItemIndex;
				for(int t=0;t<delta;t++)
				{
					RemoveInternal(0);
				}

				childIndex = items.Count;
				// create and measure new children is needed
				while (itemIndex <= maxItemIndex)
				{
					item = await logFileViewModel.GetEventAsync(itemIndex);
					AddInternal(item);
					itemIndex++; 
				}

				while(items.Count>=MaxItems)
				{
					RemoveInternal(MaxItems);
				}
			}
			else
			{
				
			}


			FirstItemIndex = Position;
		}



	}
}
