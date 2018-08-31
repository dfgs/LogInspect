using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormatHandlerDesigner
{
	public class ListViewModel:IEnumerable<ViewModel>,INotifyCollectionChanged
	{
		private List<ViewModel> items;
		private IList model;

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public ListViewModel(IList Model)
		{
			ViewModel item;

			items = new List<ViewModel>();
			this.model = Model;
			foreach(object value in model)
			{
				item = new ViewModel(value);
				items.Add(item);
				item.ValueChanged += Item_ValueChanged;
			}
		}

		protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			CollectionChanged?.Invoke(this, e);
		}

		private void Item_ValueChanged(object sender, EventArgs e)
		{
			int index;
			ViewModel viewModel;

			viewModel = (ViewModel)sender;
			index = items.IndexOf(viewModel);
			model[index] = viewModel.Value;
		}

		public int IndexOf(ViewModel Item)
		{
			return items.IndexOf(Item);
		}

		public ViewModel Add(object Value)
		{
			ViewModel item;
			int index;

			index = items.Count;
			model.Add(Value);

			item = new ViewModel(Value);
			items.Add(item);
			item.ValueChanged += Item_ValueChanged;

			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));

			return item;
		}
		public void RemoveAt(int Index)
		{
			ViewModel item;

			item = items[Index];
			model.RemoveAt(Index);
			items.RemoveAt(Index);

			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove,item,Index));
		}

		public IEnumerator<ViewModel> GetEnumerator()
		{
			return items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return items.GetEnumerator();
		}

	}
}
