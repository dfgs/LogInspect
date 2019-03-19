using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.ViewModels.Filters;
using LogInspect.ViewModels.Properties;
using LogInspect.Models;
using LogLib;
using LogInspect.BaseLib.Parsers;

namespace LogInspect.ViewModels.Columns
{
	public class InlinePropertyColumnViewModel : ColumnViewModel
	{
		public override bool AllowsResize => true;
		public override bool AllowsFilter => true;


		public override bool IsImageVisible => false;
		public override string ImageSource => "/LogInspect;component/Images/Calendar.png"; // define a default image souce to avoid converter exceptions

		public IInlineParser InlineParser
		{
			get;
			private set;
		}

		public InlinePropertyColumnViewModel(ILogger Logger, string Name,string Alignment, IInlineParser InlineParser) : base(Logger,Name,Name,Alignment)
		{
			this.InlineParser = InlineParser;
		}

		public override PropertyViewModel CreatePropertyViewModel(Event Event)
		{
			return new InlinePropertyViewModel(Logger, Name, InlineParser.Parse(Event[Name] ).ToArray());
		}

		public override FilterViewModel CreateFilterViewModel()
		{
			return new InlineFilterViewModel(Logger,Name,Filter as InlineFilterViewModel);
		}

	}
}
