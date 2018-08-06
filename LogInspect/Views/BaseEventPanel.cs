using LogInspect.Models;
using LogInspect.ViewModels;
using LogInspectLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LogInspect.Views
{
	public abstract class BaseEventPanel : FrameworkElement, IScrollInfo
	{
		public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register("ItemHeight", typeof(double), typeof(BaseEventPanel), new FrameworkPropertyMetadata(16.0d, FrameworkPropertyMetadataOptions.AffectsRender, LayoutPropertyChanged));
		public double ItemHeight
		{
			get { return (double)GetValue(ItemHeightProperty); }
			set { SetValue(ItemHeightProperty, value); }
		}

		public static readonly DependencyProperty ItemsCountProperty = DependencyProperty.Register("ItemsCount", typeof(int), typeof(BaseEventPanel), new PropertyMetadata(0, LayoutPropertyChanged));
		public int ItemsCount
		{
			get { return (int)GetValue(ItemsCountProperty); }
			set { SetValue(ItemsCountProperty, value); }
		}

		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(LogFileViewModel), typeof(BaseEventPanel), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, LayoutPropertyChanged));
		public LogFileViewModel ItemsSource
		{
			get { return (LogFileViewModel)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}


		public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns", typeof(IEnumerable<ColumnViewModel>), typeof(BaseEventPanel), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
		public IEnumerable<ColumnViewModel> Columns
		{
			get { return (IEnumerable<ColumnViewModel>)GetValue(ColumnsProperty); }
			set { SetValue(ColumnsProperty, value); }
		}


		public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(int), typeof(BaseEventPanel));
		public int Position
		{
			get { return (int)GetValue(PositionProperty); }
			set { SetValue(PositionProperty, value); }
		}


		#region IScrollInfo
		public bool CanVerticallyScroll { get; set; }
		public bool CanHorizontallyScroll { get; set; }


		public static readonly DependencyProperty ExtentWidthProperty = DependencyProperty.Register("ExtentWidth", typeof(double), typeof(BaseEventPanel), new FrameworkPropertyMetadata(0.0d, FrameworkPropertyMetadataOptions.AffectsRender, ExtentWidthPropertyChanged));
		public double ExtentWidth
		{
			get { return (double)GetValue(ExtentWidthProperty); }
			set { SetValue(ExtentWidthProperty, value); }
		}



		//private Dictionary<int, double> rowHeights;
		public double ExtentHeight
		{
			get
			{
				return ItemHeight * ItemsCount;
			}
		}
		public double ViewportWidth { get; private set; }
		public double ViewportHeight { get; private set; }


		public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register("HorizontalOffset", typeof(double), typeof(BaseEventPanel), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender));
		public double HorizontalOffset
		{
			get { return (double)GetValue(HorizontalOffsetProperty); }
			set { SetValue(HorizontalOffsetProperty, value); }
		}


		public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register("VerticalOffset", typeof(double), typeof(BaseEventPanel), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender));
		public double VerticalOffset
		{
			get { return (double)GetValue(VerticalOffsetProperty); }
			set { SetValue(VerticalOffsetProperty, value); }
		}

		public ScrollViewer ScrollOwner { get; set; }
		#endregion

		private static int cacheSize = 300;
		private Cache<int, RenderTargetBitmap> rowCache;

		public BaseEventPanel()
		{
			rowCache = new Cache<int, RenderTargetBitmap>(cacheSize);

		}


		protected override Size ArrangeOverride(Size finalSize)
		{
			ViewportWidth = finalSize.Width; ViewportHeight = finalSize.Height;
			SetHorizontalOffset(HorizontalOffset); SetVerticalOffset(VerticalOffset);
			return base.ArrangeOverride(finalSize);
		}

		private static void ExtentWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((BaseEventPanel)d).OnExtentWidthPropertyChanged();
		}
		protected virtual void OnExtentWidthPropertyChanged()
		{
			rowCache.Clear();
			ScrollOwner?.InvalidateScrollInfo();
		}



		private static void LayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((BaseEventPanel)d).OnLayoutPropertyChanged();
		}
		protected virtual void OnLayoutPropertyChanged()
		{
			ScrollOwner?.InvalidateScrollInfo();
		}

		protected virtual int GetFirstItemIndex()
		{
			return (int)(VerticalOffset / ItemHeight);
		}

		//protected abstract void OnRenderColumn(DrawingContext DrawingContext, Rect Rect, EventViewModel Event,ColumnViewModel Column);
		protected abstract void OnRenderUndecodedLog(DrawingContext DrawingContext, Rect Rect, EventViewModel Event);

		private RenderTargetBitmap OnRenderRow(Size Size, EventViewModel Event)
		{
			Layout layout;
			Rect rect;

			layout = new Layout(new Rect(Size));

			DrawingVisual drawingVisual = new DrawingVisual();
			DrawingContext DrawingContext = drawingVisual.RenderOpen();

			layout = new Layout(new Rect(new Size(ExtentWidth, ItemHeight)));

			switch(Event.Severity)
			{
				/*case Severity.Info:
					DrawingContext.DrawRectangle(Brushes.Yellow, null, layout.FreeRect);
					break;*/
				case Severity.Warning:
					DrawingContext.DrawRectangle(Brushes.Orange, null, layout.FreeRect);
					break;
				case Severity.Error:
					DrawingContext.DrawRectangle(Brushes.OrangeRed, null, layout.FreeRect);
					break;
				case Severity.Critical:
					DrawingContext.DrawRectangle(Brushes.Red, null, layout.FreeRect);
					break;
			}

			if (Event.Rule == null)
			{
				OnRenderUndecodedLog(DrawingContext, layout.FreeRect, Event);
			}
			else
			{
				foreach (ColumnViewModel column in Columns)
				{
					rect = layout.DockLeft(column.Width);
					DrawingContext.PushClip(new RectangleGeometry(rect));
					column.RenderEvent(DrawingContext, rect, Event);
					DrawingContext.Pop();
				}
			}

			DrawingContext.Close();

			RenderTargetBitmap bitmap = new RenderTargetBitmap((int)Size.Width, (int)Size.Height, 96, 96, PixelFormats.Default);
			bitmap.Render(drawingVisual);

			return bitmap;
		}
		protected override void OnRender(DrawingContext drawingContext)
		{
			int startEventIndex, eventIndex, y;
			int renderedCount;
			IEnumerable<EventViewModel> events;
			double dy;
			Size rowSize;
			RenderTargetBitmap img;


			dy = VerticalOffset % ItemHeight;
			renderedCount = (int)Math.Ceiling(ViewportHeight / ItemHeight);
			startEventIndex = (int)(VerticalOffset / ItemHeight);
			this.Position = startEventIndex;

			if (ItemsSource == null) return;

			events = ItemsSource.GetEvents(startEventIndex, renderedCount);

			rowSize = new Size(ExtentWidth, ItemHeight);
			y = 0; eventIndex = startEventIndex;
			foreach (EventViewModel ev in events)
			{
				if (!rowCache.TryGetValue(eventIndex, out img))
				{
					img = OnRenderRow(rowSize, ev);
					rowCache.Add(eventIndex, img);
				}

				drawingContext.DrawImage(img, new Rect(-HorizontalOffset, y * ItemHeight - dy, rowSize.Width, rowSize.Height));
				eventIndex++; y++;

			}
		}


		#region IScrollInfo

		public void LineUp()
		{
			SetVerticalOffset(VerticalOffset - ItemHeight);
		}

		public void LineDown()
		{
			SetVerticalOffset(VerticalOffset + ItemHeight);
		}

		public void LineLeft()
		{
			SetHorizontalOffset(HorizontalOffset - 1);
		}

		public void LineRight()
		{
			SetHorizontalOffset(HorizontalOffset + 1);
		}

		public void PageUp()
		{
			SetVerticalOffset(VerticalOffset - ViewportHeight / 2);
		}

		public void PageDown()
		{
			SetVerticalOffset(VerticalOffset + ViewportHeight / 2);
		}

		public void PageLeft()
		{
			SetHorizontalOffset(HorizontalOffset - ViewportWidth);
		}

		public void PageRight()
		{
			SetHorizontalOffset(HorizontalOffset + ViewportWidth);
		}

		public void MouseWheelUp()
		{
			SetVerticalOffset(VerticalOffset - ItemHeight);
		}

		public void MouseWheelDown()
		{
			SetVerticalOffset(VerticalOffset + ItemHeight);
		}

		public void MouseWheelLeft()
		{
			SetHorizontalOffset(HorizontalOffset - 10);
		}

		public void MouseWheelRight()
		{
			SetHorizontalOffset(HorizontalOffset + 10);
		}

		public void SetHorizontalOffset(double offset)
		{
			if (offset < 0 || ViewportWidth >= ExtentWidth)
			{
				offset = 0;
			}
			else
			{
				if (offset + ViewportWidth >= ExtentWidth)
				{
					offset = ExtentWidth - ViewportWidth;
				}
			}

			HorizontalOffset = (int)offset;

			ScrollOwner?.InvalidateScrollInfo();
		}

		public void SetVerticalOffset(double offset)
		{
			if (offset < 0 || ViewportHeight >= ExtentHeight)
			{
				offset = 0;
			}
			else
			{
				if (offset + ViewportHeight >= ExtentHeight)
				{
					offset = ExtentHeight - ViewportHeight;
				}
			}

			VerticalOffset = (int)offset;

			ScrollOwner?.InvalidateScrollInfo();
		}

		public Rect MakeVisible(Visual visual, Rect rectangle)
		{
			//throw new NotImplementedException();
			return Rect.Empty;
		}
		#endregion


	}

}
