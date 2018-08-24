using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.ViewModels
{
	public delegate void FilterChoiceAddedEventHandler(object sender, FilterChoiceAddedEventArgs e);

	public class FilterChoiceAddedEventArgs : EventArgs
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
		public FilterChoiceAddedEventArgs(string Property,object Value)
		{
			this.Property = Property;this.Value = Value;
		}
	}
}
