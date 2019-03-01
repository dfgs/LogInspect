using LogInspectLib;
using LogInspectLib.Readers;
using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public class LogBufferModule : BufferModule<Log>, ILogBufferModule
	{
		private ILogReader reader;
		public override bool CanProcess => reader.CanRead;

		public LogBufferModule( ILogger Logger, int LookupRetryDelay,ILogReader Reader) : base( Logger, LookupRetryDelay)
		{
			this.reader = Reader;
		}

		protected override Log OnGetItem()
		{
			return reader.Read();
		}

	}
}
