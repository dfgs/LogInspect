using LogInspectLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Models
{
	public interface ILogFile
	{
		string FileName
		{
			get;
		}
		IEnumerable<Event> Events
		{
			get;
		}



	}
}
