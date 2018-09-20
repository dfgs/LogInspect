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

		//public abstract long Position { get; }
		//public abstract long Length { get; }

		public abstract bool CanLoad
		{
			get;
		}


		public T this[int Index]
		{
			get
			{
				if (Index >= items.Count) throw new EndOfStreamException();
				return items[Index];
			}
		}

		public int Count => items.Count;


		public Loader()
		{
			items = new List<T>();
		}

		protected abstract T OnLoad();

		public void Load()
		{
			T item;
			item = OnLoad();
			items.Add(item);
		}

	}
}
