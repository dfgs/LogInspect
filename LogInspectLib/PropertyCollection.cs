using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLib
{
	public class PropertyCollection<T>:IEnumerable<T>
	{
		private Dictionary<string, T> items;

		public int Count
		{
			get {return items.Count;}
		}

		public T this[string Name]
		{
			get
			{
				if (Name == null) return default(T);
				T result;
				items.TryGetValue(Name, out result);
				return result;
			}
			set
			{
				items[Name] = value;
			}
		}

		public PropertyCollection()
		{
			items = new Dictionary<string, T>();
		}

		public IEnumerator<T> GetEnumerator()
		{
			return items.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return items.Values.GetEnumerator();
		}
	}
}
