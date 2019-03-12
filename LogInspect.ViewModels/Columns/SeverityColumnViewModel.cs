using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.Models;
using LogInspect.Models.Filters;
using LogInspect.Modules;
using LogInspect.ViewModels.Filters;
using LogInspect.ViewModels.Properties;
using LogInspect.Models.Parsers;
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
			return new SeverityPropertyViewModel(Logger, Name,  Event[Name], colorProviderModule.GetBackground(Event));
		}


	}
}
