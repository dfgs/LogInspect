using LogInspectLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public delegate void IndexedEventHandler<TInput, TIndexed>(object sender, IndexedEventArgs<TInput,TIndexed> e);

	public class IndexedEventArgs<TInput,TIndexed>:EventArgs
	{
		public TInput Input
		{
			get;
			private set;
		}

		public TIndexed IndexedItem
		{
			get;
			private set;
		}
		public IndexedEventArgs(TInput Input,TIndexed IndexedItem)
		{
			this.Input=Input;this.IndexedItem = IndexedItem;
		}
	}


}
