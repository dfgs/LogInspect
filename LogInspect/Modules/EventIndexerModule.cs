using LogInspect.Models;
using LogInspectLib;
using LogInspectLib.Readers;
using LogLib;
using ModuleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogInspect.Modules
{

	public class EventIndexerModule : BaseEventModule<FileIndex>
	{
		private EventReader eventReader;


		private int lineIndex;

		public override long Position
		{
			get { return eventReader.Position; }
		}
		public override long Target
		{
			get { return eventReader.Length; }
		}

		public EventIndexerModule(ILogger Logger, EventReader EventReader,int LookupRetryDelay) : base("EventIndexer",Logger,ThreadPriority.Lowest,LookupRetryDelay)
		{
			lineIndex = 0;
			this.eventReader = EventReader;
		}

		protected override bool MustIndexItem(FileIndex Item)
		{
			return true;
		}
		protected override FileIndex OnReadItem()
		{
			Event ev;
			long pos;
			FileIndex fileIndex;

			pos = eventReader.Position;
			ev = eventReader.Read();

			fileIndex=new FileIndex(pos, lineIndex, IndexedEventsCount, ev.Severity,ev.TimeStamp);
			lineIndex++;

			return fileIndex;
		}

		



	}
}
