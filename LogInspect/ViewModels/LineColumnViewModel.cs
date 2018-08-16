using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LogInspect.Views;
using LogInspectLib;
using LogLib;

namespace LogInspect.ViewModels
{
	public class LineColumnViewModel : ColumnViewModel
	{
		private static Brush brush = Brushes.WhiteSmoke;
		private static Pen pen = new Pen(new SolidColorBrush(Color.FromArgb(85,0,0,0)),1);

		public LineColumnViewModel(ILogger Logger,string Name) : base(Logger,Name,"Right")
		{
		}

		public override object GetValue(EventViewModel Event)
		{
			return Event.LineIndex + 1;
		}
		


	}
}
