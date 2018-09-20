using LogInspectLib;
using LogInspectLib.Loaders;
using LogInspectLib.Parsers;
using LogLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspectLib.Loaders
{
	public class EventLoader:Loader<Event>,IEventLoader
	{
		private ILogLoader logLoader;
		private int logIndex;
		private ILogParser logParser;

		public override bool CanLoad
		{
			get { return logIndex<logLoader.Count; }
		}


		public EventLoader(ILogLoader LogLoader,ILogParser LogParser)
		{
			if (LogLoader == null) throw new ArgumentNullException("LogLoader");
			if (LogParser == null) throw new ArgumentNullException("LogParser");
			this.logLoader = LogLoader;
			this.logParser = LogParser;
		}

		protected override Event OnLoad()
		{
			Log log;
			Event ev;
			int backupIndex;

			backupIndex = logIndex;

			try
			{
				do
				{
					log = logLoader[logIndex];
					ev = logParser.Parse(log);
					logIndex++;
				} while (false); // TODO add discard event rule
			
				return ev;
			}
			catch(Exception ex)
			{
				logIndex = backupIndex;
				throw (ex);
			}

		}



	}
}
