using LogInspect.ViewModels;
using LogInspectLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace LogInspect.Views
{
	public class EventPanel : VirtualizingStackPanel
	{
		public EventPanel()
		{
			DataContextChanged += EventPanel_DataContextChanged;
		}

		private void EventPanel_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			IVirtualCollection<Event> collection;
			collection = e.OldValue as IVirtualCollection<Event>;
			if (collection != null) collection.CountChanged -= Collection_CountChanged;
			collection = e.NewValue as IVirtualCollection<Event>;
			if (collection != null) collection.CountChanged += Collection_CountChanged;
		}

		private void Collection_CountChanged(object sender, EventArgs e)
		{
			ScrollOwner.InvalidateScrollInfo();
			InvalidateMeasure();
			
		}



	}

	/*public class EventPanel: FrameworkElement, IScrollInfo
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

		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IVirtualCollection), typeof(EventPanel), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, LayoutPropertyChanged));
		public IVirtualCollection ItemsSource
		{
			get { return (IVirtualCollection)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		#region IScrollInfo
		public bool CanVerticallyScroll { get; set; }
		public bool CanHorizontallyScroll { get; set; }

		public double ExtentWidth
		{
			get { return 100; }
		}

		//private Dictionary<int, double> rowHeights;
		public double ExtentHeight
		{
			get
			{
				return ItemHeight*ItemsCount;
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
			private set { SetValue(VerticalOffsetProperty, value); }
		}

		public ScrollViewer ScrollOwner { get; set; }
		#endregion

		public EventPanel()
		{

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

		protected override void OnRender(DrawingContext drawingContext)
		{
			int startIndex;
			int renderedCount;
			FormattedText text;
			IEnumerable<Event> events;
			int y;
			double dy;
			Layout layout;
			Point pos;
			Rect rect;

			dy = VerticalOffset % ItemHeight;
			renderedCount = (int)(ViewportHeight / ItemHeight);
			startIndex = (int)(VerticalOffset / ItemHeight);

			if (ItemsSource == null) return;

			events = ItemsSource.GetEvents(startIndex, renderedCount);

			y = 0;
			foreach(Event ev in events)
			{
				layout = new Layout(new Rect(0, y * ItemHeight - dy, this.RenderSize.Width, ItemHeight));
				if (ev.Rule == null)
				{
					text = Layout.FormatText(ev.Log.ToSingleLine(), Brushes.Black);
					pos = layout.GetTextPosition(text, HorizontalAlignment.Left, VerticalAlignment.Center);
					drawingContext.DrawText(text, pos);
				}
				else
				{
					foreach(Property property in ev.Properties)
					{
						rect = layout.DockLeft(100);
						text = Layout.FormatText(property.Value.ToString(), Brushes.Black,16,100);
						pos = Layout.GetTextPosition(rect, text, HorizontalAlignment.Left, VerticalAlignment.Center);
						drawingContext.DrawText(text, pos);
					}
				}
				y++;

			}
		}


		#region IScrollInfo

		public void LineUp()
		{
			SetVerticalOffset(VerticalOffset - 1);
		}

		public void LineDown()
		{
			SetVerticalOffset(VerticalOffset + 1);
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
			SetVerticalOffset(VerticalOffset - ViewportHeight);
		}

		public void PageDown()
		{
			SetVerticalOffset(VerticalOffset + ViewportHeight);
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
			SetVerticalOffset(VerticalOffset + ItemHeight);
		}

		public void MouseWheelDown()
		{
			SetVerticalOffset(VerticalOffset - ItemHeight);
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


	}*/
}
