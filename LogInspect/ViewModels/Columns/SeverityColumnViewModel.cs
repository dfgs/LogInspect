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


		


		public SeverityColumnViewModel(ILogger Logger,string Name, string Alignment,IInlineParser InlineParser, FilterItemSourcesViewModel FilterChoicesViewModel) : base(Logger,Name,Alignment,InlineParser,FilterChoicesViewModel)
		{
		
		}

		public override PropertyViewModel CreatePropertyViewModel(EventViewModel Event)
		{
			return new SeverityPropertyViewModel(Logger, Name,Alignment, InlineParser, Event.GetEventValue(Name),Event.Background,Event.Foreground);
		}


	}
}
