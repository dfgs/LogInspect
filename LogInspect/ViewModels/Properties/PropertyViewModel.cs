using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.ViewModels.Columns;
using LogInspectLib;
using LogLib;

namespace LogInspect.ViewModels.Properties
{
	public abstract class PropertyViewModel : ViewModel
	{


		public ColumnViewModel Column
		{
			get;
			private set;
		}

		/*public EventViewModel Event
		{
			get;
			private set;
		}

		public object Value
		{
			get;
			private set;
		}*/

		public PropertyViewModel(ILogger Logger,ColumnViewModel Column) : base(Logger)
		{
			this.Column = Column;
			/*this.Event = Event;
			this.Value = Column.GetValue(Event);*/
		}


	}
}
