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
	//TODO Remove ?
	public abstract class PropertyViewModel : ViewModel
	{


		private ColumnViewModel column;

		public string Name
		{
			get { return column.Name; }
		}

		public TextAlignment Alignment
		{
			get { return column.Alignment; }
		}


		public PropertyViewModel(ILogger Logger,ColumnViewModel Column) : base(Logger,-1)
		{
			this.column = Column;
		}


	}
}
