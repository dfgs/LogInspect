using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.Modules;
using LogLib;

namespace LogInspect.ViewModels
{
	public class SeveritiesViewModel : ViewModel,IEnumerable<object>,INotifyCollectionChanged
	{
		private List<object> items;
		private string severityProperty;


		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(SeveritiesViewModel));
		public object SelectedItem
		{
			get { return GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}


		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public SeveritiesViewModel(ILogger Logger,string SeverityProperty, SelectionFiltersIndexerModule SelectionFiltersIndexerModule) : base(Logger)
		{
			items = new List<object>();
			this.severityProperty = SeverityProperty;
			SelectionFiltersIndexerModule.Indexed += SelectionFiltersIndexerModule_Indexed;
		}

		private void SelectionFiltersIndexerModule_Indexed(object sender, IndexedEventArgs<string, object> e)
		{
			Dispatcher.Invoke(() => OnEventIndexed(e.Input, e.IndexedItem));
		}

		protected void OnEventIndexed(string Property,object Value)
		{
			if (Property != severityProperty) return;
			items.Add(Value);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Value));
			if (items.Count == 1) SelectedItem = Value;
			
		}

		protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			Dispatcher.Invoke(() => CollectionChanged?.Invoke(this, e));

		}

		public IEnumerator<object> GetEnumerator()
		{
			return items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return items.GetEnumerator();
		}


	}
}
