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
	public abstract class ListModule<T> : BaseModule,IListModule<T>
		where T:class
	{
			

		private List<T> items;
		public override int Count
		{
			get
			{
				return items.Count;
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
				return items[Index];
			}
		}

		public ListModule(string Name,ILogger Logger,int LookupRetryDelay,WaitHandle LookUpRetryEvent) :base(Name,Logger,LookupRetryDelay, LookUpRetryEvent, System.Threading.ThreadPriority.Lowest)
		{
			this.items = new List<T>();
		}

		protected abstract IEnumerable<T> OnGetItems();

		protected override int OnProcess()
		{
			int result;

			result = 0;
			try
			{
				foreach (T item in OnGetItems())
				{
					items.Add(item);
					result++;
					proceededCount++;
				}
				return result;
			}
			catch(Exception ex)
			{
				Log(ex);
				return result;
			}

		}
		

	}
}
