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
		
		public Inline[] Inlines
		{
			get;
			private set;
		}

		public TextPropertyViewModel(ILogger Logger, string Name, TextAlignment Alignment, string Value) : base(Logger, Name,Alignment)
		{
			this.Value = Value;this.Inlines = null;// Event.GetPropertyInlines(Column.Name);
		}
	}
}
