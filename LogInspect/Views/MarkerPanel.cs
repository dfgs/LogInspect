using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LogInspect.Views
{
	public class MarkerPanel:Panel
    {

		public static readonly DependencyProperty TotalSizeProperty = DependencyProperty.Register("TotalSize",typeof(double), typeof(MarkerPanel),new FrameworkPropertyMetadata(0.0d,FrameworkPropertyMetadataOptions.AffectsMeasure));
		public double TotalSize
		{
			get { return (double)GetValue(TotalSizeProperty); }
			set { SetValue(TotalSizeProperty, value); }
		}


		public static readonly DependencyProperty MinItemHeightProperty = DependencyProperty.Register("MinItemHeight", typeof(double), typeof(MarkerPanel), new FrameworkPropertyMetadata(0.0d, FrameworkPropertyMetadataOptions.AffectsMeasure));
		public double MinItemHeight
		{
			get { return (double)GetValue(MinItemHeightProperty); }
			set { SetValue(MinItemHeightProperty, value); }
		}

		public static readonly DependencyProperty PositionProperty = DependencyProperty.RegisterAttached("Position", typeof(double), typeof(MarkerPanel), new FrameworkPropertyMetadata(0.0d, FrameworkPropertyMetadataOptions.AffectsParentArrange));
		public static readonly DependencyProperty SizeProperty = DependencyProperty.RegisterAttached("Size", typeof(double), typeof(MarkerPanel), new FrameworkPropertyMetadata(0.0d, FrameworkPropertyMetadataOptions.AffectsMeasure));


		public static void SetPosition(DependencyObject Component, double Value)
		{
			Component.SetValue(PositionProperty, Value);
		}
		public static double GetPosition(DependencyObject Component)
		{
			return (double)Component.GetValue(PositionProperty);
		}

		public static void SetSize(DependencyObject Component, double Value)
		{
			Component.SetValue(SizeProperty, Value);
		}
		public static double GetSize(DependencyObject Component)
		{
			return (double)Component.GetValue(SizeProperty);
		}


		protected override Size MeasureOverride(Size availableSize)
		{
			double size;
			double h;

			foreach(UIElement child in Children)
			{
				size = GetSize(child);
				h = Math.Max(MinItemHeight, availableSize.Height * size / TotalSize);
				child.Measure(new Size(availableSize.Width,h));
			}

			return availableSize;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			double size;
			double position;
			double h,y;

			foreach (UIElement child in Children)
			{
				position = GetPosition(child);
				size = GetSize(child);
				h = finalSize.Height * size / TotalSize+MinItemHeight;
				y = position * finalSize.Height / TotalSize-MinItemHeight*0.5d;
				child.Arrange(new Rect(0,y,finalSize.Width, h));
			}

			return DesiredSize; 
		}




	}
}
