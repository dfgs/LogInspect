using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FormatHandlerDesigner.Views
{
	public delegate void AddItemEventHandler(object sender, AddItemEventArgs e);

	public class AddItemEventArgs:EventArgs
	{

		public object AddedItem
		{
			get;
			set;
		}
		public AddItemEventArgs()
		{

		}

	}
}
