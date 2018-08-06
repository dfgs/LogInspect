using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LogInspect.Views
{
	public static class Options
	{
		public static Brush EventBackground = Brushes.White;
		public static Brush EventForeground = Brushes.Black;
		public static Brush LineIndexForeground = Brushes.Gray;
		public static Brush ColumnHeaderBackground = Brushes.WhiteSmoke;
		public static Brush BorderBrush = new SolidColorBrush(Color.FromArgb(85,0,0,0));
	}
}
