using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.ViewModels.Columns;
using LogInspectLib;
using LogInspectLib.Parsers;
using LogLib;

namespace LogInspect.ViewModels.Properties
{
	public class TextPropertyViewModel : PropertyViewModel
	{
		
		public IEnumerable<Inline> Inlines
		{
			get;
			private set;
		}

		public TextPropertyViewModel(ILogger Logger, string Name, TextAlignment Alignment, IInlineParser InlineParser, string Value) : base(Logger, Name,Alignment)
		{
			this.Value = Value;this.Inlines = InlineParser.Parse(Value);
		}
	}
}
