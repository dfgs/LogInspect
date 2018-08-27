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
	public class TextPropertyViewModel : PropertyViewModel
	{
		public string Value
		{
			get;
			private set;
		}


		public Inline[] Inlines
		{
			get;
			private set;
		}

		public TextPropertyViewModel(ILogger Logger, ColumnViewModel Column,EventViewModel Event) : base(Logger, Column)
		{
			this.Value = Event.GetPropertyValue(Column.Name)?.ToString();this.Inlines = Event.GetPropertyInlines(Column.Name);
		}
	}
}
