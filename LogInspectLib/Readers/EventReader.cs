using LogInspectLib.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogInspectLib.Readers
{
    public class EventReader :Reader<Event>,IEventReader
    {
		public override bool EndOfStream
		{
			get { return logReader.EndOfStream; }
		}

		public override long Position
		{
			get { return logReader.Position; }
		}
		public override long Length
		{
			get { return logReader.Length; }
		}

		private ILogReader logReader;

		private IEnumerable<ILogParser> logParsers;

		
		public EventReader(ILogReader LogReader, IEnumerable<ILogParser> LogParsers):base()
        {
			if (LogReader == null) throw new ArgumentNullException("LogReader");

			this.logReader = LogReader;

			logParsers = LogParsers ?? Enumerable.Empty<ILogParser>();			
		}

		protected override void OnSeek(long Position)
		{
			logReader.Seek(Position);
		}

		
		protected override Event OnRead()
		{
			Log log;
			Event? ev;
			//Event result;

			log = logReader.Read();
			foreach (LogParser parser in logParsers)
			{
				ev = parser.Parse(log);
				if (ev.HasValue) return ev.Value;
			}
			return new Event(log, null, Property.EmptyProperties);

		}
		

	}
}
