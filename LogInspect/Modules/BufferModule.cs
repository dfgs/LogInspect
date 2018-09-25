using LogInspectLib;
using LogInspectLib.Readers;
using LogLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public abstract class BufferModule<T> : BaseModule,IBufferModule<T>
	{
		

		private List<T> items;
		public override int Count
		{
			get
			{
				lock(items)
				{
					return items.Count;
				}
			}
		}

		private int proceededCount;
		public override int ProceededCount
		{
			get { return proceededCount; }
		}

		public T this[int Index]
		{
			get
			{
				lock(items)
				{
					return items[Index];
				}
			}
		}

		

		public BufferModule(string Name,ILogger Logger,int LookupRetryDelay) :base(Name,Logger,LookupRetryDelay, null, System.Threading.ThreadPriority.Lowest)
		{
			this.items = new List<T>();
		}

		protected abstract T OnGetItem();

		public IEnumerable<T> GetBuffer()
		{
			IEnumerable<T> result;

			lock(items)
			{
				result = items;
				items = new List<T>();
			}
			return result;
		}
		protected override int OnProcess()
		{
			T item;

			try
			{
				item = OnGetItem();
				lock(items)
				{
					items.Add(item);
					proceededCount++;
				}
				return 1;
			}
			catch(Exception ex)
			{
				Log(ex);
				return 0;
			}
			
		}
		

	}
}
