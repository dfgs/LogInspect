using System;
using System.Collections.Async;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogLib;

namespace LogInspect.ViewModels
{
	public abstract class FilteredCollectionViewModel<TSource, T> : CollectionViewModel<T>
		where T:class
	{
		public FilteredCollectionViewModel(ILogger Logger) : base(Logger)
		{
			
		}
		protected abstract IEnumerable<T> Filter(IEnumerable<TSource> Items);

		
		public async Task Load(IEnumerable<TSource> Items)
		{
			IEnumerable<T> filteredItems;

			filteredItems = await Task.Run(() => Filter(Items));

			Load(filteredItems);
		}
		

	}
}
