using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LogInspect.Views;
using LogLib;

namespace LogInspect.ViewModels
{
	public class PropertyColumnViewModel : ColumnViewModel
	{
		

		public PropertyColumnViewModel(ILogger Logger,string Name,string Alignment) : base(Logger,Name,Alignment)
		{
		}

		

		public override object GetValue(EventViewModel Event)
		{
			return Event.GetPropertyValue(Name);
		}


	}
}
