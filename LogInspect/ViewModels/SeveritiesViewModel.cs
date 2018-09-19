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
	public class SeveritiesViewModel : CollectionViewModel<object>
	{
		private int position;
		private IEnumerable<object> items;

		public SeveritiesViewModel(ILogger Logger, int RefreshInterval, string SeverityColumn, FilterItemSourcesViewModel FilterChoicesViewModel) : base(Logger,RefreshInterval)
		{
			items=FilterChoicesViewModel[SeverityColumn];
		}

		protected override void OnRefresh()
		{
			
			int count;

			count = items.Count();

			for (int t = position; t < count; t++)
			{
				Add(items.ElementAt(t));
			}
			if ((Count > 0) && (SelectedItem == null)) SelectedItem = this[0];
			position = count;
			
		}
		/*private void FilterChoicesViewModel_FilterChoiceAdded(object sender, FilterItemAddedEventArgs e)
		{
			if (e.Property != severityProperty) return;
			Add(e.Value);
			if (Count == 1) SelectedItem = e.Value;
			
		}*/



	}
}
