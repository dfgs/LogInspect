using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public delegate void SeverityAddedEventHandler(object sender, SeverityAddedEventArgs e);

	public class SeverityAddedEventArgs:EventArgs
	{
		public string Severity
		{
			get;
			private set;
		}

		public SeverityAddedEventArgs(string Severity)
		{
			this.Severity = Severity;
		}
	}


}
