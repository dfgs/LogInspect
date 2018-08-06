using LogInspect.Models;
using LogInspect.ViewModels;
using LogInspectLib;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LogInspect.Views
{
	public class EventPanel : BaseEventPanel
	{
		/*protected override void OnRenderColumn(DrawingContext DrawingContext, Rect Rect,EventViewModel Event,ColumnViewModel Column)
		{
			object value;
			FormattedText text;
			Point pos;

			value = Column.GetValue(Event);
			if (value == null) return;
			text = DrawUtils.FormatText(value.ToString(), Brushes.Black, 16, Rect.Width);
			pos = DrawUtils.GetTextPosition(Rect, text, HorizontalAlignment.Left, VerticalAlignment.Center);
			DrawingContext.DrawText(text, pos);
		}*/

	

		protected override void OnRenderUndecodedLog(DrawingContext DrawingContext, Rect Rect, EventViewModel Event)
		{
			FormattedText text;
			Point pos;

			text = DrawUtils.FormatText(Event.RawLog, Brushes.White);
			pos = DrawUtils.GetTextPosition(Rect, text, HorizontalAlignment.Left, VerticalAlignment.Center);
			DrawingContext.DrawText(text, pos);
		}





	}
}
