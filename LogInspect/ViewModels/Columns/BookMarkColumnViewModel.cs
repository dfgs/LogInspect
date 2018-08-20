using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LogInspect.ViewModels.Properties;
using LogInspect.Views;
using LogInspectLib;
using LogLib;

namespace LogInspect.ViewModels.Columns
{
	public class BookMarkColumnViewModel : ColumnViewModel
	{
		public override bool AllowResize => false;

		public BookMarkColumnViewModel(ILogger Logger,string Name) : base(Logger,Name)
		{
		}

		public override PropertyViewModel CreatePropertyViewModel(EventViewModel Event)
		{
			return new BookMarkPropertyViewModel(Logger, this, Event);
		}



	}
}
