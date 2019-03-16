using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspect.Models
{
	public class NameSpaceDictionary<T>:INameSpaceDictionary<T>
	{
		private Dictionary<string, T> items;
		private Dictionary<string, T> fullNamedItems;

		public int Count
		{
			get { return items.Count; }
		}

		public NameSpaceDictionary()
		{
			fullNamedItems = new Dictionary<string, T>();
			items = new Dictionary<string, T>();
		}

		public static string GetFullName(string NameSpace,string Name)
		{
			return $"{NameSpace}.{Name}";
		}

		public void Add(string NameSpace,string Name,T Item)
		{
			string fullName;

			fullName = GetFullName(NameSpace, Name);
			if (fullNamedItems.ContainsKey(fullName)) fullNamedItems.Remove(fullName);
			fullNamedItems.Add(fullName, Item);
			if (items.ContainsKey(Name)) items.Remove(Name);
			items.Add(Name, Item);
		}


		public T GetItem(string Name)
		{
			T item;

			if (fullNamedItems.TryGetValue(Name, out item)) return item;
			if (items.TryGetValue(Name, out item)) return item;

			throw new KeyNotFoundException($"Item {Name} doesn't exist in dictionary");
		}

		public T GetItem(string DefaultNameSpace,string Name)
		{
			string fullName;
			T item;

			if (fullNamedItems.TryGetValue(Name, out item)) return item;
			fullName = GetFullName(DefaultNameSpace, Name);
			if (fullNamedItems.TryGetValue(fullName, out item)) return item;
			if (items.TryGetValue(Name, out item)) return item;

			throw new KeyNotFoundException($"Item {Name} doesn't exist in dictionary");
		}

	}
}
