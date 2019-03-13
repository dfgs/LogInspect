using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.Models.Filters;
using LogInspect.ViewModels.Filters;
using LogInspect.ViewModels.Properties;
using LogInspect.Models;
using LogLib;

namespace LogInspect.ViewModels.Columns
{
	public class BookMarkColumnViewModel : ColumnViewModel
	{
		public override bool AllowsResize => false;
		public override bool AllowsFilter => false;

		public override bool IsImageVisible => true;
		public override string ImageSource => "/LogInspect;component/Images/flag_blue.png";

		public BookMarkColumnViewModel(ILogger Logger,string Name) : base(Logger,Name,"","Center")
		{
		}

		public override PropertyViewModel CreatePropertyViewModel(Event Event)
		{
			return new BookMarkPropertyViewModel(Logger,Name);
		}

		public override FilterViewModel CreateFilterViewModel()
		{
			return null;
		}

	}
}
