﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LogInspect.Views;
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

		public override void RenderEvent(DrawingContext DrawingContext, Rect Rect, EventViewModel Event)
		{
			FormattedText text;
			Point pos;


			DrawingContext.DrawRectangle(brush, null, Rect);
			//DrawingContext.DrawLine(pen, Rect.BottomLeft, Rect.BottomRight);

			Rect.Inflate(-10, 0);
			text = DrawUtils.FormatText((Event.LineIndex+1).ToString(), Brushes.Gray, 16, Rect.Width);
			pos = DrawUtils.GetTextPosition(Rect, text, Alignment, VerticalAlignment.Center);
			DrawingContext.DrawText(text, pos);

		}

	}
}