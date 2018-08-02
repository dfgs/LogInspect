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
	public class EventPanel : FrameworkElement, IScrollInfo
	{
		public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register("ItemHeight", typeof(double), typeof(EventPanel), new FrameworkPropertyMetadata(16.0d, FrameworkPropertyMetadataOptions.AffectsRender, LayoutPropertyChanged));
		public double ItemHeight
		{
			get { return (double)GetValue(ItemHeightProperty); }
			set { SetValue(ItemHeightProperty, value); }
		}

		public static readonly DependencyProperty ItemsCountProperty = DependencyProperty.Register("ItemsCount", typeof(int), typeof(EventPanel), new PropertyMetadata(0, LayoutPropertyChanged));
		public int ItemsCount
		{
			get { return (int)GetValue(ItemsCountProperty); }
			set { SetValue(ItemsCountProperty, value); }
		}

		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(LogFileViewModel), typeof(EventPanel), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, LayoutPropertyChanged));
		public LogFileViewModel ItemsSource
		{
			get { return (LogFileViewModel)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}


		public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns", typeof(IEnumerable<ColumnViewModel>), typeof(EventPanel), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
		public IEnumerable<ColumnViewModel> Columns
		{
			get { return (IEnumerable<ColumnViewModel>)GetValue(ColumnsProperty); }
			set { SetValue(ColumnsProperty, value); }
		}

		#region IScrollInfo
		public bool CanVerticallyScroll { get; set; }
		public bool CanHorizontallyScroll { get; set; }

		public double ExtentWidth
		{
			get { return ViewportWidth; }
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


		public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register("HorizontalOffset", typeof(double), typeof(EventPanel), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender));
		public double HorizontalOffset
		{
			get { return (double)GetValue(HorizontalOffsetProperty); }
			private set { SetValue(HorizontalOffsetProperty, value); }
		}


		public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register("VerticalOffset", typeof(double), typeof(EventPanel), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender));
		public double VerticalOffset
		{
			get { return (double)GetValue(VerticalOffsetProperty); }
			set { SetValue(VerticalOffsetProperty, value); }
		}

		public ScrollViewer ScrollOwner { get; set; }
		#endregion

		private static int cacheSize = 300;
		private Cache<int, RenderTargetBitmap> rowCache;

		public EventPanel()
		{
			rowCache = new Cache<int, RenderTargetBitmap>(cacheSize);
			
		}


		protected override Size ArrangeOverride(Size finalSize)
		{
			ViewportWidth = finalSize.Width; ViewportHeight = finalSize.Height;
			SetHorizontalOffset(HorizontalOffset); SetVerticalOffset(VerticalOffset);
			return base.ArrangeOverride(finalSize);
		}

		private static void LayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((EventPanel)d).OnLayoutPropertyChanged();
		}
		protected virtual void OnLayoutPropertyChanged()
		{
			ScrollOwner?.InvalidateScrollInfo();
			//InvalidateVisual();
		}

		protected virtual int GetFirstItemIndex()
		{
			return (int)(VerticalOffset / ItemHeight);
		}

		private RenderTargetBitmap OnRenderRow( Size Size,EventViewModel Event)
		{
			FormattedText text;
			Layout layout;
			Point pos;
			Rect rect;
			string value;

			layout = new Layout(new Rect(Size));

			DrawingVisual drawingVisual = new DrawingVisual();
			DrawingContext DrawingContext = drawingVisual.RenderOpen();

			rect = layout.DockLeft(50);
			text = Layout.FormatText(Event.LineIndex.ToString(), Brushes.Black);
			pos = Layout.GetTextPosition(rect, text, HorizontalAlignment.Left, VerticalAlignment.Center);
			DrawingContext.DrawText(text, pos);


			if (Event.Rule == null)
			{
				text = Layout.FormatText(Event.Message.ToString(), Brushes.Black);
				pos = layout.GetTextPosition(text, HorizontalAlignment.Left, VerticalAlignment.Center);
				DrawingContext.DrawText(text, pos);
			}
			else
			{
				foreach (ColumnViewModel column in Columns)
				{
					rect = layout.DockLeft(column.Width);
					value = Event.GetPropertyValue(column.Name);
					if (value == null) continue;
					text = Layout.FormatText(value, Brushes.Black, 16, column.Width);
					pos = Layout.GetTextPosition(rect, text, HorizontalAlignment.Left, VerticalAlignment.Center);
					DrawingContext.DrawText(text, pos);
				}
			}

			DrawingContext.Close();

			RenderTargetBitmap bitmap = new RenderTargetBitmap((int)Size.Width, (int)Size.Height, 96, 96, PixelFormats.Default);
			bitmap.Render(drawingVisual);

			return bitmap;
		}
		protected override void OnRender(DrawingContext drawingContext)
		{
			int startEventIndex,eventIndex,y;
			int renderedCount;
			IEnumerable<EventViewModel> events;
			double dy;
			Size rowSize;
			RenderTargetBitmap img;


			dy = VerticalOffset % ItemHeight;
			renderedCount = (int)(ViewportHeight / ItemHeight);
			startEventIndex = (int)(VerticalOffset / ItemHeight);

			if (ItemsSource == null) return;

			events = ItemsSource.GetEvents(startEventIndex, renderedCount);

			rowSize = new Size(ExtentWidth, ItemHeight);
			y = 0;eventIndex=startEventIndex ;
			foreach (EventViewModel ev in events)
			{
				if (!rowCache.TryGetValue(eventIndex,out img))
				{
					img = OnRenderRow(rowSize, ev);
					rowCache.Add(eventIndex, img);
				}

				drawingContext.DrawImage(img, new Rect(0, y*ItemHeight- dy, rowSize.Width, rowSize.Height));
				eventIndex++;y++;

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
			SetVerticalOffset(VerticalOffset - ViewportHeight/2);
		}

		public void PageDown()
		{
			SetVerticalOffset(VerticalOffset + ViewportHeight/2);
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
