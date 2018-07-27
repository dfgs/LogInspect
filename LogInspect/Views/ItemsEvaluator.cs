using LogInspect.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace LogInspect.Views
{
	public class ItemsEvaluator:DependencyObject
	{
		public static readonly DependencyProperty LogFileViewModelProperty = DependencyProperty.RegisterAttached("LogFileViewModel", typeof(LogFileViewModel), typeof(ItemsEvaluator), new PropertyMetadata(null, LogFileViewModelPropertyChanged));
		public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.RegisterAttached("ItemHeight", typeof(double), typeof(ItemsEvaluator), new PropertyMetadata(16.0d, ItemHeightPropertyChanged));


		public static LogFileViewModel GetLogFileViewModel(DependencyObject Component)
		{
			return (LogFileViewModel)Component.GetValue(LogFileViewModelProperty);
		}
		public static void SetLogFileViewModel(DependencyObject Component, LogFileViewModel Value)
		{
			Component.SetValue(LogFileViewModelProperty, Value);
		}
		public static double GetItemHeight(DependencyObject Component)
		{
			return (double)Component.GetValue(ItemHeightProperty);
		}
		public static void SetItemHeight(DependencyObject Component, double Value)
		{
			Component.SetValue(ItemHeightProperty, Value);
		}

		private static void LogFileViewModelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Panel)d).SizeChanged += Panel_SizeChanged;
			((Panel)d).Loaded += ItemsEvaluator_Loaded; ;
			RefreshItemCount((Panel)d);
		}

		private static void ItemHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RefreshItemCount((Panel)d);
		}

		private static void ItemsEvaluator_Loaded(object sender, RoutedEventArgs e)
		{
			RefreshItemCount((Panel)sender);
		}

		private static void Panel_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			RefreshItemCount((Panel)sender);
		}

		private static void RefreshItemCount(Panel component)
		{
			LogFileViewModel vm;
			double itemHeight;
			int count;

			vm = GetLogFileViewModel(component);
			if (vm == null) return;
			itemHeight = GetItemHeight(component);
			
			count = (int)Math.Floor(component.ActualHeight / itemHeight);
			vm.PageSize = count;
		}

	}
}
