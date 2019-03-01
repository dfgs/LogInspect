using LogInspect.ViewModels;
using LogInspect.ViewModels.Columns;
using LogInspectLib;
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
	public class EventListModule : ListModule<Event>,IEventListModule
	{
		public override bool CanProcess
		{
			get { return logBuffer.Count>0; }
		}

		private ILogBufferModule logBuffer;
		private ILogParser logParser;
		
		public EventListModule(ILogger Logger, int LookupRetryDelay, ILogBufferModule LogBuffer, ILogParser LogParser) :base(Logger,LookupRetryDelay,LogBuffer.ProceededEvent)
		{
			this.logBuffer = LogBuffer;
			this.logParser = LogParser;
		}


		protected override IEnumerable<Event> OnGetItems()
		{
			Event item;

			foreach (Log log in logBuffer.GetBuffer())
			{
				item = logParser.Parse(log);
				if (item == null) continue;
				yield return item;
			}


		}
		

	}
}
