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

		public List<Event> Events
		{
			get;
			private set;
		}
		IEnumerable<Event> ILogFile.Events => Events;



		public LogFile(string FileName)
		{
			this.FileName = FileName;this.Events = new List<Event>();
		}

		

	}
}
