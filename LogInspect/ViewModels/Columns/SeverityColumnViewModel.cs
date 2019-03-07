using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LogInspect.Models;
using LogInspect.Models.Filters;
using LogInspect.Modules;
using LogInspect.ViewModels.Filters;
using LogInspect.ViewModels.Properties;
using LogInspect.Views;
using LogInspectLib;
using LogInspectLib.Parsers;
using LogLib;

namespace LogInspect.ViewModels.Columns
{
	public class SeverityColumnViewModel : MultiChoicesColumnViewModel
	{
		private IColorProviderModule colorProviderModule;

		public SeverityColumnViewModel(ILogger Logger,string Name, string Alignment, FilterItemSourcesViewModel FilterChoicesViewModel, IColorProviderModule ColorProviderModule) : base(Logger,Name,Alignment,FilterChoicesViewModel)
		{
			AssertParameterNotNull("ColorProviderModule", ColorProviderModule);
			this.colorProviderModule = ColorProviderModule;
		}

		public override PropertyViewModel CreatePropertyViewModel(Event Event)
		{
			Brush brush, background, foreground ;

			brush = colorProviderModule.GetBackground(Event);
			if (brush==null)
			{
				background = Brushes.Transparent;foreground = Brushes.Black;
			}
			else
			{
				background = brush;foreground = brush;
			}
			return new SeverityPropertyViewModel(Logger, Name,  Event[Name],background,foreground);
		}


	}
}
