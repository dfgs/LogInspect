using LogInspect.Models;
using LogInspect.ViewModels.Columns;
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
	public abstract class CollectionViewModel<TModel, T>:ViewModel,IEnumerable<T>,INotifyCollectionChanged
		where T:class
	{
		private List<T> items;

		private T selectedItem;
		public T SelectedItem
		{
			get { return selectedItem; }
			set { selectedItem = value; OnPropertyChanged(); }
		}

		public int SelectedIndex
		{
			get { return selectedItem == null ? -1 : items.IndexOf(selectedItem); }
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
	
		public CollectionViewModel(ILogger Logger) : base(Logger)
		{
			items = new List<T>();
		}


		


		protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			CollectionChanged?.Invoke(this, e);
		}

		public virtual void Clear()
		{
			SelectedItem = null;
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
			if (item.Equals(selectedItem )) SelectedItem = items.FirstOrDefault();
		}
		public void Remove(T Item)
		{
			int index;

			index = items.IndexOf(Item);
			items.RemoveAt(index);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, Item, index));
			OnPropertyChanged("Count");
			if (Item.Equals(selectedItem)) SelectedItem = items.FirstOrDefault();
		}
		public void Add(T Item)
		{
			int index;

			index = items.Count;
			items.Add(Item);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Item, index));
			OnPropertyChanged("Count");
		}
		public void AddRange(IEnumerable<T> Items)
		{
			int index;
			foreach (T item in Items)
			{
				index = items.Count;
				items.Add(item);
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
				OnPropertyChanged("Count");
			}
		}
		public void Insert(int Index, T Item)
		{
			items.Insert(Index, Item);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Item, Index));
			OnPropertyChanged("Count");
		}
		public void Load(IEnumerable<T> Items)
		{
			items.Clear();
			items.AddRange(Items);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			OnPropertyChanged("Count");
			SelectedItem = items.FirstOrDefault();
		}

		// must not be IEnumerable in order to keep function async
		protected abstract T[] GenerateItems(IEnumerable<TModel> Items);

		public async Task LoadModels(IEnumerable<TModel> Items)
		{
			IEnumerable<T> filteredItems;

			filteredItems = await Task.Run( () => GenerateItems(Items));
			
			items.Clear();
			items.AddRange(filteredItems);

			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			OnPropertyChanged("Count");
			SelectedItem = items.FirstOrDefault();
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
