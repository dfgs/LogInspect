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
		private IEnumerable<EventViewModel> events;
		private string severityColumn;

		public SeveritiesViewModel(ILogger Logger, IEnumerable<EventViewModel> Events, string SeverityColumn) : base(Logger)
		{
			AssertParameterNotNull("Events", Events);
			AssertParameterNotNull("SeverityColumn", SeverityColumn);
			this.events = Events;
			this.severityColumn = SeverityColumn;
		}

		public void Refresh()
		{
			this.Load(events.Select((vm) => vm[severityColumn].Value.ToString()).Distinct());
		}



	}
}
