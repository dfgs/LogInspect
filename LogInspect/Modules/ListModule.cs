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
				lock(items)
				{
					return items.Count;
				}
			}
		}

		public T this[int Index]
		{
			get
			{
				lock (items)
				{
					return items[Index];
				}
			}
		}

		public ListModule(string Name,ILogger Logger,int LookupRetryDelay) :base(Name,Logger,LookupRetryDelay, null, System.Threading.ThreadPriority.Lowest)
		{
			this.items = new List<T>();
		}

		protected abstract IEnumerable<T> OnGetItems();

		protected override bool OnProcess()
		{

			try
			{
				lock (items)
				{
					foreach (T item in OnGetItems())
					{
						items.Add(item);
					}
				}
				return true;
			}
			catch(Exception ex)
			{
				Log(ex);
				return false;
			}
			
		}
		

	}
}
