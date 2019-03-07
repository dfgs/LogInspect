using LogInspect.Models;

using LogInspect.ViewModels.Columns;
using LogInspectLib;
using LogLib;
using System;
using System.Collections;
using System.Collections.Async;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace LogInspect.ViewModels
{
	public class MarkersViewModel:CollectionViewModel<EventViewModel, MarkerViewModel>
	{
		private string severityColumn;


		public MarkersViewModel(ILogger Logger ,string SeverityColumn) : base(Logger)
		{
			//AssertParameterNotNull("Events", Events);
			AssertParameterNotNull("SeverityColumn", SeverityColumn);

			this.severityColumn = SeverityColumn;
			//this.events = Events;
		}

		protected override IEnumerable<MarkerViewModel> GenerateItems(IEnumerable<EventViewModel> Items)
		{
			string severity;
			MarkerViewModel range = null;
			int index = 0;
			List<MarkerViewModel> items;

			items = new List<MarkerViewModel>();
			foreach (EventViewModel item in Items)
			{
				if (item.SeverityBrush != null)
				{
					severity = item.GetEventValue(severityColumn);
					if ((range == null) || (severity != range.Severity) || (index != range.Position + range.Size))
					{
						range = new MarkerViewModel(Logger);
						range.Position = index;
						range.Background = item.SeverityBrush;
						range.Severity = severity;
						items.Add(range);
					}
					range.Size++;
				}

				index++;
			}

			return items;
		}
		
		

	}
}
