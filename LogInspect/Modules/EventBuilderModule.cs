using LogInspect.ViewModels;
using LogInspect.ViewModels.Columns;
using LogInspectLib;
using LogInspectLib.Loaders;
using LogInspectLib.Parsers;
using LogLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public class EventBuilderModule : BaseModule,IEventBuilderModule
	{
		public override bool CanProcess
		{
			get { return logLoader.Count>0; }
		}

		private ILogLoader logLoader;
		private ILogParser logParser;

		private IEnumerable<ColumnViewModel> columns;
		private IEnumerable<EventColoringRule> eventColoringRules;

		private List<EventViewModel> items;


		public EventBuilderModule(ILogger Logger, int LookupRetryDelay, ILogLoader LogLoader, ILogParser LogParser, IEnumerable<ColumnViewModel> Columns, IEnumerable<EventColoringRule> EventColoringRules) :base("EventBuilder",Logger,LookupRetryDelay, null, System.Threading.ThreadPriority.Lowest)
		{
			this.items = new List<EventViewModel>();

			this.columns = Columns;
			this.eventColoringRules = EventColoringRules;

			this.logLoader = LogLoader;
			this.logParser = LogParser;
		}

		protected override bool OnProcessItem()
		{
			EventViewModel vm;
			Event ev;


			foreach (Log log in logLoader.GetBuffer())
			{
				ev = logParser.Parse(log);
				if (ev == null) continue;
				vm = new EventViewModel(Logger, columns, eventColoringRules, ev);
				vm.EventIndex = Count;
				items.Add(vm);
			}

			return true;

		}
		

	}
}
