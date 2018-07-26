using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using LogInspectLib;
using LogLib;

namespace LogInspect.ViewModels
{
	public abstract class VirtualCollection<T> : ViewModel, IVirtualCollection<T>
	{
		private int pageCount;
		private int pageSize;
		private List<Tuple<int,T[]>> pages;

		public T this[int index]
		{
			get
			{
				int pageIndex = index % pageSize;
				T[] page = GetPage(index);;
				return page[pageIndex];
			}
			set
			{
				;
			}
		}

		object IList.this[int index]
		{
			get { return this[index]; }
			set { this[index] = (T)value; }
		}



		
		public int Count
		{
			get;
			private set;
		}

		public bool IsReadOnly
		{
			get { return true; }
		}

		public bool IsFixedSize => throw new NotImplementedException();

		public object SyncRoot => throw new NotImplementedException();

		public bool IsSynchronized => throw new NotImplementedException();

		public event EventHandler CountChanged;
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public VirtualCollection(ILogger Logger,int PageCount,int PageSize) : base(Logger)
		{
			this.pages = new List<Tuple<int, T[]>>();
			this.pageCount = PageCount;
			this.pageSize = PageSize;
		}


		public void SetCount(int Value)
		{
			this.Count = Value;
			CountChanged?.Invoke(this, EventArgs.Empty);
		}

		private T[] GetPage(int ItemIndex)
		{
			T[] page;
			int pageIndex;
			
			pageIndex= ItemIndex / pageSize;
			foreach(Tuple<int,T[]> tuple in pages)
			{
				if (tuple.Item1 == pageIndex) return tuple.Item2;
			}

			return LoadPage(pageIndex);
		}
		private T[] LoadPage(int PageIndex)
		{
			T[] page;
			
			page = OnLoadPage(PageIndex,pageSize).ToArray();
			if (pages.Count == pageCount) pages.RemoveAt(pageCount - 1);
			pages.Insert(0, new Tuple<int, T[]>(PageIndex, page));
			return page;
		}
		protected abstract IEnumerable<T> OnLoadPage(int PageIndex,int PageSize);

		protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			CollectionChanged?.Invoke(this, e);
		}
		
		public void Add(T item)
		{
			throw new NotImplementedException();
		}

		public int Add(object value)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public bool Contains(T item)
		{
			return true;
			foreach (Tuple<int, T[]> tuple in pages)
			{
				for (int t = 0; t < pageSize; t++)
				{
					if (item.Equals(tuple.Item2[t]))
						return true;
				}
			}
			return false;
		}

		public bool Contains(object value)
		{
			if (value == null) return false;
			return Contains((T)value);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public void CopyTo(Array array, int index)
		{
			throw new NotImplementedException();
		}

		public IEnumerator<T> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		public int IndexOf(T item)
		{
			return -1;
			foreach(Tuple<int,T[]> tuple in pages)
			{
				for(int t=0;t<pageSize;t++)
				{
					if (item.Equals(tuple.Item2[t]))
						return tuple.Item1 * pageSize + t;
				}
			}
			return -1;
		}

		public int IndexOf(object value)
		{
			return IndexOf((T)value);
		}

		public void Insert(int index, T item)
		{
			throw new NotImplementedException();
		}

		public void Insert(int index, object value)
		{
			throw new NotImplementedException();
		}

		public bool Remove(T item)
		{
			throw new NotImplementedException();
		}

		public void Remove(object value)
		{
			throw new NotImplementedException();
		}

		public void RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			yield break;
		}

		

		


	}
}
