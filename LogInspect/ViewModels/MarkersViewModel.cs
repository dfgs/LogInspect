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

		public int Maximum
		{
			get;
			private set;
		}
		

		public MarkersViewModel(ILogger Logger , int RefreshInterval, EventLoaderModule EventLoaderModule, IEnumerable<EventColoringRule> ColoringRules,string SeverityColumn) : base(Logger, RefreshInterval)
		{
			//BufferModule.EventsBuffered += BufferModule_EventsBuffered;
			//BufferModule.Reseted += BufferModule_Reseted;
			this.severityColumn = SeverityColumn;
			this.coloringRules = ColoringRules;
		}


		/*private void BufferModule_Reseted(object sender, EventArgs e)
		{
			Dispatcher.Invoke(() =>
			{
				Clear();
			});
		}*/

		/*private void BufferModule_EventsBuffered(object sender, EventsBufferedEventArgs e)
		{
			object severity;
			MarkerViewModel range=null;
			Brush brush;
			

			Dispatcher.Invoke(() =>
			{
				Maximum += e.Items.Length;
				foreach (BufferItem item in e.Items)
				{
					brush = EventViewModel.GetBackground(coloringRules, item.Event);
					if (brush == null) continue;

					severity = item.Event[severityColumn];
					if (this.Count > 0) range = this[Count - 1];

					if ((range == null) || (!ValueType.Equals(severity, range.Severity) || (item.EventIndex != range.Position + range.Size)))
					{
						range = new MarkerViewModel(Logger);
						range.Position = item.EventIndex;
						range.Background = brush;
						range.Severity = severity;
						Add(range);
					}

					range.Size++;
				}
			});
		}*/


	}
}
