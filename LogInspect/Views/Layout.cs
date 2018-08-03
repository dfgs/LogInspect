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
	public class Layout
	{


		public Rect FreeRect
		{
			get;
			private set;
		}

		public Layout(Rect Rect)
		{
			FreeRect = Rect;

		}

		
	
		/*public Point GetTextPosition(FormattedText Text, HorizontalAlignment HorizontalAlignment, VerticalAlignment VerticalAlignment)
		{
			return Drawing.GetTextPosition(FreeRect, Text, HorizontalAlignment, VerticalAlignment);
		}*/




		public void Trim(double Size)
		{
			FreeRect = Trim(FreeRect, Size);
		}
		public void Trim(double Left, double Top, double Right, double Bottom)
		{
			FreeRect = Trim(FreeRect, Left, Top, Right, Bottom);
		}

		public static Rect Trim(Rect Source, double Size)
		{
			return new Rect(Source.Left + Size, Source.Top + Size, Source.Width - 2 * Size, Source.Height - 2 * Size);
		}
		public static Rect Trim(Rect Source, double Left, double Top, double Right, double Bottom)
		{
			return new Rect(Source.Left + Left, Source.Top + Top, Source.Width - Left - Right, Source.Height - Top - Bottom);
		}

		public Rect DockTop(double Height)
		{
			Rect result;

			if (Height > FreeRect.Height) Height = FreeRect.Height;
			result = new Rect(FreeRect.Left, FreeRect.Top, FreeRect.Width, Height);
			FreeRect = new Rect(FreeRect.Left, FreeRect.Top + Height, FreeRect.Width, FreeRect.Height - Height);

			return result;
		}
		public Rect DockBottom(double Height)
		{
			Rect result;

			if (Height > FreeRect.Height) Height = FreeRect.Height;
			result = new Rect(FreeRect.Left, FreeRect.Top + FreeRect.Height - 1 - Height, FreeRect.Width, Height);
			FreeRect = new Rect(FreeRect.Left, FreeRect.Top, FreeRect.Width, FreeRect.Height - Height);

			return result;
		}

		public Rect DockLeft(double Width)
		{
			Rect result;
			if (Width > FreeRect.Width) Width = FreeRect.Width;
			result = new Rect(FreeRect.Left, FreeRect.Top, Width, FreeRect.Height);
			FreeRect = new Rect(FreeRect.Left + Width, FreeRect.Top, FreeRect.Width - Width, FreeRect.Height);

			return result;
		}
		public Rect DockRight(double Width)
		{
			Rect result;

			if (Width > FreeRect.Width) Width = FreeRect.Width;
			result = new Rect(FreeRect.Left + FreeRect.Width - 1 - Width, FreeRect.Top, Width, FreeRect.Height);
			FreeRect = new Rect(FreeRect.Left, FreeRect.Top, FreeRect.Width - Width, FreeRect.Height);

			return result;
		}

		public Rect SplitTop()
		{
			return DockTop(FreeRect.Height / 2.0d);
		}
		public Rect SplitBottom()
		{
			return DockBottom(FreeRect.Height / 2.0d);
		}
		public Rect SplitLeft()
		{
			return DockLeft(FreeRect.Width / 2.0d);
		}
		public Rect SplitRight()
		{
			return DockRight(FreeRect.Width / 2.0d);
		}

		public static Rect Center(Rect Source, double Width, double Height)
		{
			return new Rect(Source.Left + (Source.Width - Width) / 2, Source.Top + (Source.Height - Height) / 2, Width, Height);
		}

		public static Point GetCenter(Rect Source)
		{
			return new Point(Source.Left + Source.Width / 2, Source.Top + Source.Height / 2);
		}

	}

}
