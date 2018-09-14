using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLib
{
	public class PropertyCollection<T>
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

	}
}
