using LogInspect.Models;
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
		where T:class
	{
		private List<T> items;

		private T selectedItem;
		public T SelectedItem
		{
			get { return selectedItem; }
			set { selectedItem = value; OnPropertyChanged(); }
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

		/*public void AddRange(IList<T> Items)
		{
			int index;

			index = items.Count;
			items.AddRange(Items);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,(IList)Items, index));
			OnPropertyChanged("Count");
		}*/


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
