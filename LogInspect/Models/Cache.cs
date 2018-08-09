using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Models
{
	public class Cache<TKey,TValue>
	{
		private int size;

		private List<TKey> keys;
		private Dictionary<TKey, TValue> dictionary;

		public TValue this[TKey Key]
		{
			get { return dictionary[Key]; }
		}

		public Cache(int Size)
		{
			this.size = Size;
			keys = new List<TKey>(size);
			dictionary = new Dictionary<TKey, TValue>(size);
		}

		public void Add(TKey Key,TValue Value)
		{
			if (keys.Count==size)
			{
				dictionary.Remove(keys[0]);
				keys.RemoveAt(0);
			}
			keys.Add(Key);
			dictionary.Add(Key, Value);
		}
		public void Remove(TKey Key)
		{

			keys.Remove(Key);
			dictionary.Remove(Key);
			
		}

		public bool TryGetValue(TKey Key,out TValue Value)
		{
			return dictionary.TryGetValue(Key, out Value);
		}

		public void Clear()
		{
			keys.Clear();
			dictionary.Clear();
		}

	}

}
