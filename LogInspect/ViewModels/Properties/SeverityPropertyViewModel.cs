using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LogInspect.ViewModels.Columns;
using LogInspectLib.Parsers;
using LogLib;

namespace LogInspect.ViewModels.Properties
{
	public class SeverityPropertyViewModel : PropertyViewModel
	{
		public Brush Background
		{
			get;
			private set;
		}
		public Brush Foreground
		{
			get;
			private set;
		}

		public SeverityPropertyViewModel(ILogger Logger, string Name,   string Value,Brush Background,Brush Foreground) : base(Logger, Name,Value)
		{
			this.Background = Background;this.Foreground = Foreground;
		}
	}
}
