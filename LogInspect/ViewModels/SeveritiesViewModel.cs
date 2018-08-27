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
	public class SeveritiesViewModel : BaseCollectionViewModel<object>
	{
		private string severityProperty;

		
		public SeveritiesViewModel(ILogger Logger,string SeverityProperty, FilterItemSourcesViewModel FilterChoicesViewModel) : base(Logger)
		{
			
			this.severityProperty = SeverityProperty;

			FilterChoicesViewModel.FilterChoiceAdded += FilterChoicesViewModel_FilterChoiceAdded;
		}

		private void FilterChoicesViewModel_FilterChoiceAdded(object sender, FilterItemAddedEventArgs e)
		{
			if (e.Property != severityProperty) return;
			Add(e.Value);
			if (Count == 1) SelectedItem = e.Value;
			
		}



	}
}
