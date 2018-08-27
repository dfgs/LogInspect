using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.ViewModels
{
	public delegate void FilterItemAddedEventHandler(object sender, FilterItemAddedEventArgs e);

	public class FilterItemAddedEventArgs : EventArgs
	{
		public string Property
		{
			get;
			private set;
		}
		public object Value
		{
			get;
			private set;
		}
		public FilterItemAddedEventArgs(string Property,object Value)
		{
			this.Property = Property;this.Value = Value;
		}
	}
}
