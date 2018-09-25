using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LogInspect.Views
{
	public class ProgressPanel:Panel
	{

		public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached("Value", typeof(double), typeof(ProgressPanel),new FrameworkPropertyMetadata(0d,FrameworkPropertyMetadataOptions.AffectsParentArrange));
		public static readonly DependencyProperty MaximumProperty = DependencyProperty.RegisterAttached("Maximum", typeof(double), typeof(ProgressPanel), new FrameworkPropertyMetadata(100d,FrameworkPropertyMetadataOptions.AffectsParentArrange));

		public static double GetValue(DependencyObject Component)
		{
			return (double)Component.GetValue(ValueProperty);
		}
		public static void SetValue(DependencyObject Component,double Value)
		{
			Component.SetValue(ValueProperty, Value);
		}
		public static double GetMaximum(DependencyObject Component)
		{
			return (double)Component.GetValue(MaximumProperty);
		}
		public static void SetMaximum(DependencyObject Component, double Value)
		{
			Component.SetValue(MaximumProperty, Value);
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			double width;
			double value, maximum;

			foreach(UIElement element in Children)
			{
				value = GetValue(element);
				maximum = Math.Max(1, GetMaximum(element));
				width = availableSize.Width * value / maximum;
				element.Measure(new Size(width, availableSize.Height));
			}

			return availableSize;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			double width;
			double value, maximum;

			foreach (UIElement element in Children)
			{
				value = GetValue(element);
				maximum = Math.Max(1, GetMaximum(element));
				width = finalSize.Width * value / maximum;

				element.Arrange(new Rect(0,0,width,finalSize.Height));
			}
			return finalSize;
		}

	}
}
