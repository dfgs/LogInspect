using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLib.Loaders
{
	public abstract class Loader<T> : ILoader<T>
	{
		private List<T> items;

		/*public abstract bool CanLoad
		{
			get;
		}*/


		/*public T this[int Index]
		{
			get
			{
				lock (items)
				{
					return items[Index];
				}
			}
		}*/

		public int Count
		{
			get
			{
				lock(items)
				{
					return items.Count;
				}
			}
		}


		public Loader()
		{
			items = new List<T>();
		}

		protected abstract T OnLoad();

		public void Load()
		{
			T item;
			item = OnLoad();
			lock (items)
			{
				items.Add(item);
			}
		}

		public IEnumerable<T> GetBuffer()
		{
			IEnumerable<T> result;
			lock (items)
			{
				result = items;
				items = new List<T>();
			}
			return items;
		}

	}
}
