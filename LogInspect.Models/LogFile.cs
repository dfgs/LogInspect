using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogInspect.Models;

namespace LogInspect.Models
{
	public class LogFile 
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
		public EventList Events
		{
			get;
			private set;
		}



		public LogFile(string FileName,FormatHandler FormatHandler)
		{
			if (FileName == null) throw new ArgumentNullException("FileName");
			if (FormatHandler == null) throw new ArgumentNullException("FormatHandler");

			this.FileName = FileName;
			this.FormatHandler = FormatHandler;

			this.Events = new EventList();
		}

		

	}
}
