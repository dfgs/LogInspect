using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using LogLib;

namespace LogInspect.ViewModels
{
	public class SeveritiesViewModel : CollectionViewModel<string>
	{
		private int position;
		private IEnumerable<string> items;

		public SeveritiesViewModel(ILogger Logger,  string SeverityColumn, FilterItemSourcesViewModel FilterChoicesViewModel) : base(Logger)
		{
			items = FilterChoicesViewModel[SeverityColumn];//.Cast<string>();
		}

		protected override void OnRefresh()
		{
			
			int count;
			if (items == null) return;

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
