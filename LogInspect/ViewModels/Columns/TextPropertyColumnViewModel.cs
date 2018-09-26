using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
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
	public class TextPropertyColumnViewModel : ColumnViewModel
	{
		public override bool AllowsResize => true;
		public override bool AllowsFilter => true;


		public override Visibility ImageVisibility => Visibility.Collapsed;
		public override string ImageSource => "/LogInspect;component/Images/Calendar.png"; // define a default image souce to avoid converter exceptions

		public IInlineParser InlineParser
		{
			get;
			private set;
		}

		public TextPropertyColumnViewModel(ILogger Logger, string Name,string Alignment, IInlineParser InlineParser) : base(Logger,Name,Name,Alignment,null)
		{
			this.InlineParser = InlineParser;
		}

		public override PropertyViewModel CreatePropertyViewModel(EventViewModel Event)
		{
			return new TextPropertyViewModel(Logger, Name,Alignment, InlineParser, Event.GetEventValue(Name));
		}

		public override FilterViewModel CreateFilterViewModel()
		{
			
			return new TextFilterViewModel(Logger,Name,(TextFilter)Filter);
		}

	}
}
