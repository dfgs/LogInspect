using LogInspect.BaseLib.Parsers;
using LogInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.BaseLib.Readers
{
	public class EventReader : Reader<Event>, IEventReader
	{
		private ILogReader logReader;
		private ILogParser logParser;

		public override bool CanRead => logReader.CanRead;

		public EventReader(ILogReader LogReader,ILogParser LogParser)
		{
			if (LogReader == null) throw new ArgumentNullException("LogReader");
			if (LogParser == null) throw new ArgumentNullException("LogParser");
			this.logReader = LogReader;
			this.logParser = LogParser;
		}

		protected override Event OnRead()
		{
			Log log;

			log = logReader.Read();
			return logParser.Parse(log);
		}


	}
}
