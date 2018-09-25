using LogInspect.Models;
using LogInspect.Modules;
using LogInspect.ViewModels.Columns;
using LogInspectLib;
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
	public class CollectionViewModel<T>:ViewModel,IEnumerable<T>,INotifyCollectionChanged
	{
		private List<T> items;


		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(T), typeof(CollectionViewModel<T>));
		public T SelectedItem
		{
			get { return (T)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		public T this[int Index]
		{
			get { return items[Index]; }
		}

		public int Count
		{
			get { return items.Count; }
		}


		public event NotifyCollectionChangedEventHandler CollectionChanged;
	
		public CollectionViewModel(ILogger Logger,int RefreshInterval) : base(Logger, RefreshInterval)
		{
			items = new List<T>();
		}

	

		protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			CollectionChanged?.Invoke(this, e);
		}

		public void Clear()
		{
			items.Clear();
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			OnPropertyChanged("Count");
		}
		public void Remove(int Index)
		{
			T item;

			item = items[Index];
			items.RemoveAt(Index);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, Index));
			OnPropertyChanged("Count");
		}
		
		public void Add(T Item)
		{
			int index;

			index = items.Count;
			items.Add(Item);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Item, index));
			OnPropertyChanged("Count");
		}
		public void AddRange(IList<T> Items)
		{
			int index;

			index = items.Count;
			items.AddRange(Items);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,(IList)Items, index));
			OnPropertyChanged("Count");
		}
		public void Insert(int Index, T Item)
		{
			items.Insert(Index, Item);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Item, Index));
			OnPropertyChanged("Count");
		}

		public void Select(int Index)
		{
			SelectedItem = items[Index];
		}


		public IEnumerator<T> GetEnumerator()
		{
			return items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return items.GetEnumerator();
		}




	}
}
