using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FormatHandlerDesigner.Views
{
	public delegate void GetPatternEventHandler(object sender, GetPatternEventArgs e);

	public class GetPatternEventArgs : EventArgs
	{

		public string Pattern
		{
			get;
			set;
		}
		public GetPatternEventArgs()
		{

		}

	}
}
