using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogInspectLib;

namespace LogInspect.Models
{
	public class LogFile : ILogFile
	{
		
		public string FileName
		{
			get;
			private set;
		}
		public FormatHandler FormatHandler
		{
			get;
			private set;
		}
		public List<Event> Events
		{
			get;
			private set;
		}
		IEnumerable<Event> ILogFile.Events => Events;



		public LogFile(string FileName,FormatHandler FormatHandler)
		{
			if (FileName == null) throw new ArgumentNullException("FileName");
			if (FormatHandler == null) throw new ArgumentNullException("FormatHandler");

			this.FileName = FileName;
			this.FormatHandler = FormatHandler;

			this.Events = new List<Event>();
		}

		

	}
}
