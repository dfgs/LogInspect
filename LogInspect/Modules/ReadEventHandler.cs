using LogInspectLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.Modules
{
	public delegate void ReadEventHandler<TInput>(object sender, ReadEventArgs<TInput> e);

	public class ReadEventArgs<TInput>:EventArgs
	{
		public TInput Input
		{
			get;
			private set;
		}

		public ReadEventArgs(TInput Input)
		{
			this.Input=Input;
		}
	}


}
