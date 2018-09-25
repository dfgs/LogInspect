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
using System.Windows.Media;

namespace LogInspect.ViewModels
{
	public class MarkersViewModel:CollectionViewModel<MarkerViewModel>
	{
		private string severityColumn;
		private IEnumerable<EventColoringRule> coloringRules;

		private int position;


		private FilteredEventsViewModel events;


		public int Maximum
		{
			get;
			private set;
		}
		

		public MarkersViewModel(ILogger Logger , int RefreshInterval, FilteredEventsViewModel Events, IEnumerable<EventColoringRule> ColoringRules,string SeverityColumn) : base(Logger, RefreshInterval)
		{
			this.severityColumn = SeverityColumn;
			this.coloringRules = ColoringRules;
			this.events = Events;
		}
		protected override void OnRefresh()
		{
			int count;
			EventViewModel item;
			string severity;
			MarkerViewModel range = null;
			Brush brush;

			lock (this)
			{
				count = events.Count;

			
				for (int t = position; t < count; t++)
				{
					item = events[t];
					brush = EventViewModel.GetBackground(coloringRules, item);
					if (brush == null) continue;

					severity = item.GetEventValue(severityColumn);
					if (this.Count > 0) range = this[Count - 1];

					if ((range == null) || (severity != range.Severity) || (t != range.Position + range.Size))
					{
						range = new MarkerViewModel(Logger);
						range.Position = t;
						range.Background = brush;
						range.Severity = severity;
						Add(range);
					}

					range.Size++;
				}
				position = count;
			}
		}

		public void Clear()
		{
			lock(this)
			{
				this.position = 0;
				Reset();
			}
		}

	}
}
