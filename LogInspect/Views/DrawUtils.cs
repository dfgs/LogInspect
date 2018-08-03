using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace LogInspect.Views
{
	public static class DrawUtils
	{
		public static FlowDirection FlowDirection = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
		public static Typeface Typeface = new Typeface("Helvetica");//Segoe UI

		public static SolidColorBrush TransparentDarkBrush = new SolidColorBrush(Color.FromArgb(92, 0, 0, 0));
		public static SolidColorBrush TransparentLightBrush = new SolidColorBrush(Color.FromArgb(92, 255, 255, 255));

		public static Pen TransparentDarkPen = new Pen(TransparentDarkBrush, 1);
		public static Pen TransparentLightPen = new Pen(TransparentLightBrush, 1);


		public static FormattedText FormatText(string Text, Brush Foreground, double Size = 16, double? MaxWidth = null)
		{
			FormattedText text;

			text = new FormattedText(Text, CultureInfo.CurrentCulture, FlowDirection, Typeface, Size, Foreground);
			text.Trimming = TextTrimming.CharacterEllipsis; text.MaxLineCount = 1;
			if (MaxWidth.HasValue) text.SetMaxTextWidths(new double[] { MaxWidth.Value });

			return text;
		}

		public static Point GetTextPosition(Rect Rect, FormattedText Text, HorizontalAlignment HorizontalAlignment, VerticalAlignment VerticalAlignment)
		{
			double x, y;

			switch (HorizontalAlignment)
			{
				case HorizontalAlignment.Left:
					x = Rect.Left;
					break;
				case HorizontalAlignment.Right:
					x = Rect.Right - Text.WidthIncludingTrailingWhitespace;
					break;
				case HorizontalAlignment.Center:
					x = Rect.Left + (Rect.Width - Text.WidthIncludingTrailingWhitespace) / 2;
					break;
				default:
					x = Rect.Left;
					break;
			}
			switch (VerticalAlignment)
			{
				case VerticalAlignment.Top:
					y = Rect.Top;
					break;
				case VerticalAlignment.Bottom:
					y = Rect.Bottom - Text.Height;
					break;
				case VerticalAlignment.Center:
					y = Rect.Top + (Rect.Height - Text.Height) / 2;
					break;
				default:
					y = Rect.Top;
					break;
			}


			return new Point(x, y);
		}
		public static void RenderBevel(DrawingContext Context, Rect Rect)
		{
			Context.DrawLine(DrawUtils.TransparentDarkPen, Rect.TopRight, Rect.BottomRight);
			Context.DrawLine(DrawUtils.TransparentDarkPen, Rect.BottomLeft, Rect.BottomRight + new Vector(-1, 0));
			Context.DrawLine(DrawUtils.TransparentLightPen, Rect.TopLeft + new Vector(1, 1), Rect.TopRight + new Vector(-1, 1));
			Context.DrawLine(DrawUtils.TransparentLightPen, Rect.TopLeft + new Vector(1, 1), Rect.BottomLeft + new Vector(1, -1));//*/
		}
		public static void RenderInvertedBevel(DrawingContext Context, Rect Rect)
		{
			Context.DrawLine(DrawUtils.TransparentLightPen, Rect.TopRight, Rect.BottomRight);
			Context.DrawLine(DrawUtils.TransparentLightPen, Rect.BottomLeft, Rect.BottomRight + new Vector(-1, 0));
			Context.DrawLine(DrawUtils.TransparentDarkPen, Rect.TopLeft + new Vector(0, 1), Rect.TopRight + new Vector(-1, 1));
			Context.DrawLine(DrawUtils.TransparentDarkPen, Rect.TopLeft + new Vector(1, 1), Rect.BottomLeft + new Vector(1, -1));
		}
	}
}
