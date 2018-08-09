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
using System.Windows.Input;
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

		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(LogFileViewModel), typeof(BaseEventPanel), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, ItemsSourcePropertyChanged));
		public LogFileViewModel ItemsSource
		{
			get { return (LogFileViewModel)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		//private int selectedItemIndex;
		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(EventViewModel), typeof(BaseEventPanel), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender,SelectedItemPropertyChanged));
		public EventViewModel SelectedItem
		{
			get { return (EventViewModel)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
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


		public static readonly DependencyProperty MaxRenderedItemsProperty = DependencyProperty.Register("MaxRenderedItems", typeof(int), typeof(BaseEventPanel));
		public int MaxRenderedItems
		{
			get { return (int)GetValue(MaxRenderedItemsProperty); }
			private set { SetValue(MaxRenderedItemsProperty, value); }
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
			Focusable = true;
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


		private static void ItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((BaseEventPanel)d).OnItemsSourceChanged((LogFileViewModel)e.OldValue, (LogFileViewModel)e.NewValue);
		}
		protected virtual void OnItemsSourceChanged(LogFileViewModel OldValue,LogFileViewModel NewValue)
		{
			if (OldValue!=null) OldValue.PagesCleared-= ItemsSource_PagesCleared;
			if (NewValue != null) NewValue.PagesCleared += ItemsSource_PagesCleared;
			//OnLayoutChanged();
		}

		private void ItemsSource_PagesCleared(object sender, EventArgs e)
		{
			rowCache.Clear();
			InvalidateVisual();
			//SelectedItem = null;
		}

		private static void LayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((BaseEventPanel)d).OnLayoutChanged();
		}
		protected virtual void OnLayoutChanged()
		{
			ScrollOwner?.InvalidateScrollInfo();
			if (ItemsCount < Position + MaxRenderedItems) InvalidateVisual() ;
		}

		protected virtual int GetFirstItemIndex()
		{
			return (int)(VerticalOffset / ItemHeight);
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			MaxRenderedItems = (int)Math.Floor(ViewportHeight / ItemHeight);
			return base.MeasureOverride(availableSize);
		}


		protected abstract void OnRenderUndecodedLog(DrawingContext DrawingContext, Rect Rect, EventViewModel Event);

		private RenderTargetBitmap OnRenderRow(Size Size, EventViewModel Event)
		{
			Layout layout;
			Rect rect;

			layout = new Layout(new Rect(Size));

			DrawingVisual drawingVisual = new DrawingVisual();
			DrawingContext DrawingContext = drawingVisual.RenderOpen();

			layout = new Layout(new Rect(new Size(ExtentWidth, ItemHeight)));

			if (SelectedItem == Event)
			{
				DrawingContext.DrawRectangle(Brushes.LightSteelBlue, null, layout.FreeRect);
			}
			else
			{
				DrawingContext.DrawRectangle(Event.Background, null, layout.FreeRect);
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
			int eventIndex, y;
			IEnumerable<EventViewModel> events;
			double dy;
			Size rowSize;
			RenderTargetBitmap img;
			Rect rowRect;

			dy = VerticalOffset % ItemHeight;

			if (ItemsSource == null) return;

			events = ItemsSource.GetEvents(Position, MaxRenderedItems);

			rowSize = new Size(ExtentWidth, ItemHeight);
			y = 0; eventIndex = Position;
			foreach (EventViewModel ev in events)
			{
				if (!rowCache.TryGetValue(eventIndex, out img))
				{
					img = OnRenderRow(rowSize, ev);
					rowCache.Add(eventIndex, img);
				}

				rowRect = new Rect(-HorizontalOffset, y * ItemHeight - dy, rowSize.Width, rowSize.Height);
				drawingContext.DrawImage(img, rowRect);
				

				eventIndex++; y++;

			}
		}



		private static void SelectedItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((BaseEventPanel)d).OnSelectedItemChanged((EventViewModel)e.OldValue,(EventViewModel)e.NewValue);
		}

		protected virtual void OnSelectedItemChanged(EventViewModel OldValue, EventViewModel NewValue)
		{
			if (OldValue!=null) rowCache.Remove(OldValue.EventIndex);
			if (NewValue!=null) rowCache.Remove(NewValue.EventIndex);

			// auto scroll to selected event
			if ((NewValue.EventIndex< Position) || (NewValue.EventIndex > Position + MaxRenderedItems ))
			{
				SetVerticalOffset(NewValue.EventIndex * ItemHeight);
			}
		}

		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			double dy;
			Point position;
			int index;

			e.Handled = true;

			position = Mouse.GetPosition(this);

			dy = VerticalOffset % ItemHeight;
			index = (int)(Position + (position.Y + dy) / ItemHeight);

			Focus();
			if (e.LeftButton != MouseButtonState.Pressed) return;
			SelectedItem = ItemsSource.GetEvent(index);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			int newIndex;

			if (SelectedItem == null) return;

			e.Handled = true;
			switch(e.Key)
			{
				case Key.PageUp:
					newIndex = SelectedItem.EventIndex - MaxRenderedItems;
					if (newIndex <0) newIndex = 0;
					SetVerticalOffset(VerticalOffset - MaxRenderedItems * ItemHeight);
					SelectedItem = ItemsSource.GetEvent(newIndex);
					break;
				case Key.Up:
					newIndex = SelectedItem.EventIndex - 1;
					if (newIndex < 0) newIndex = 0;
					if (newIndex < Position) SetVerticalOffset(newIndex * ItemHeight);
					SelectedItem = ItemsSource.GetEvent(newIndex);
					break;
				case Key.PageDown:
					newIndex = SelectedItem.EventIndex + MaxRenderedItems;
					if (newIndex > ItemsCount - 1) newIndex = ItemsCount - 1;
					SetVerticalOffset(VerticalOffset + MaxRenderedItems * ItemHeight);
					SelectedItem = ItemsSource.GetEvent(newIndex);
					break;
				case Key.Down:
					newIndex = SelectedItem.EventIndex + 1;
					if (newIndex > ItemsCount - 1) newIndex = ItemsCount - 1;
					if (newIndex > Position + MaxRenderedItems - 1) SetVerticalOffset((newIndex - MaxRenderedItems + 1) * ItemHeight);
					SelectedItem = ItemsSource.GetEvent(newIndex);
					break;
				default:
					e.Handled = false;
					return;
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
			Position = (int)(VerticalOffset / ItemHeight);

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
