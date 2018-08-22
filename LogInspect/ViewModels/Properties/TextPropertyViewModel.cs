using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.ViewModels.Columns;
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

		public TextAlignment Alignment
		{
			get;
			private set;
		}
		public TextPropertyViewModel(ILogger Logger, ColumnViewModel Column,EventViewModel Event,TextAlignment Alignment) : base(Logger, Column)
		{
			this.Value = Event.GetPropertyValue(Column.Name)?.ToString();this.Alignment = Alignment;
		}
	}
}
