using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LogInspect.ViewModels.Columns;
using LogLib;

namespace LogInspect.ViewModels.Properties
{
	public class SeverityPropertyViewModel : TextPropertyViewModel
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

		public SeverityPropertyViewModel(ILogger Logger, ColumnViewModel Column,EventViewModel Event,TextAlignment Alignment) : base(Logger, Column,Event,Alignment)
		{
			this.Background = Event.Background;this.Foreground = Event.Foreground;
		}
	}
}
