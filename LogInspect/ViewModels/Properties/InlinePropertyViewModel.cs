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
	public class InlinePropertyViewModel : PropertyViewModel
	{
		
		

		public InlinePropertyViewModel(ILogger Logger, string Name,  IEnumerable<Inline> Inlines) : base(Logger, Name,Inlines)
		{
		}

		public override string ToString()
		{
			IEnumerable<Inline> inlines;

			inlines=Value as IEnumerable<Inline>;

			return string.Join(" ", inlines.Select((item) => item.Value));
		}


	}
}
