using LogInspect.Modules;
using LogLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogInspect.ViewModels
{
	public class SeverityIndexerViewModel : IndexerModuleViewModel<SeverityIndexerModule, string>,IEnumerable<SeverityViewModel>,INotifyCollectionChanged
	{
		private List<SeverityViewModel> items;


		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(SeverityViewModel), typeof(SeverityIndexerViewModel));
		public SeverityViewModel SelectedItem
		{
			get { return (SeverityViewModel)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}



		public event NotifyCollectionChangedEventHandler CollectionChanged;
		//public event EventHandler IsCheckedChanged;



		public SeverityIndexerViewModel(ILogger Logger, SeverityIndexerModule IndexerModule,  int RefreshInterval) : base(Logger, IndexerModule, RefreshInterval)
		{
			items = new List<SeverityViewModel>();
		}

		protected override void OnRefresh()
		{
			SeverityViewModel item;
			int index;

			while(items.Count<IndexerModule.IndexedEventsCount)
			{
				index = items.Count;
				item = new SeverityViewModel(Logger, IndexerModule[items.Count]);
				//item.IsCheckedChanged += Item_IsCheckedChanged;
				items.Add(item);
				CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
				if (items.Count == 1) SelectedItem = item;
			}
		}

		/*private void Item_IsCheckedChanged(object sender, EventArgs e)
		{
			IsCheckedChanged?.Invoke(this, e);
		}*/

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
