using LogInspect.Modules;
using LogLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.ViewModels
{
	public class SeverityIndexerViewModel : IndexerModuleViewModel<SeverityIndexerModule, string>,IEnumerable<SeverityViewModel>,INotifyCollectionChanged
	{
		private List<SeverityViewModel> items;

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public SeverityIndexerViewModel(ILogger Logger, SeverityIndexerModule IndexerModule, int RefreshInterval) : base(Logger, IndexerModule, RefreshInterval)
		{
			items = new List<SeverityViewModel>();
		}

		protected override void OnRefresh()
		{
			SeverityViewModel item;
			int index;

			while(items.Count<IndexerModule.IndexedEvents)
			{
				index = items.Count;
				item = new SeverityViewModel(Logger, IndexerModule[items.Count]);
				items.Add(item);
				CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
			}
		}

		public IEnumerator<SeverityViewModel> GetEnumerator()
		{
			foreach(SeverityViewModel item in items) yield return item;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

	}
}
