using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogLib;

namespace LogInspect.ViewModels
{
	public class CollectionViewModel<TViewModel,TModel> : ViewModel,IEnumerable<TViewModel>
		where TViewModel :ViewModel
	{
		private IEnumerable<TModel> model;
		private List<TViewModel> items;
		private Func<TModel, TViewModel> createViewModelItem;

		public CollectionViewModel(ILogger Logger,IEnumerable<TModel> Model,Func<TModel,TViewModel> CreateViewModelItem) : base(Logger)
		{
			INotifyCollectionChanged notifyCollection;

			items = new List<TViewModel>();

			this.createViewModelItem = CreateViewModelItem;
			this.model = Model;
			notifyCollection = model as INotifyCollectionChanged;
			if (notifyCollection!=null) notifyCollection.CollectionChanged += NotifyCollection_CollectionChanged;

			foreach (TModel item in model) items.Add(createViewModelItem(item));
		}


		private void NotifyCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			throw new NotImplementedException();
		}


		public IEnumerator<TViewModel> GetEnumerator()
		{
			foreach (TViewModel item in items) yield return item;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}



	}
}
