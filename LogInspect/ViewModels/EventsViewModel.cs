using LogInspect.Models;
using LogInspect.Modules;
using LogInspect.ViewModels.Columns;
using LogInspectLib;
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
	public class EventsViewModel:BaseCollectionViewModel<EventViewModel>
	{
		
		private IEnumerable<ColumnViewModel> columns;
		private IEnumerable<ColoringRule> coloringRules;

		public EventsViewModel(ILogger Logger ,EventIndexerModule IndexerModule,IEnumerable<ColumnViewModel> Columns,IEnumerable<ColoringRule> ColoringRules) : base(Logger)
		{
			IndexerModule.Indexed += IndexerModule_Indexed;
			IndexerModule.Reseted += IndexerModule_Reseted;
			this.columns = Columns;
			this.coloringRules = ColoringRules;
		}

		private void IndexerModule_Reseted(object sender, EventArgs e)
		{
			Dispatcher.Invoke(() =>
			{
				Clear();
			});
		}

		private void IndexerModule_Indexed(object sender, EventIndexedEventArgs e)
		{
			EventViewModel ev;
			Dispatcher.Invoke(() =>
			{
				ev = new EventViewModel(Logger, columns, coloringRules, e.Event, e.EventIndex, e.LineIndex);
				Add(ev);
			});
		}


	}
}
