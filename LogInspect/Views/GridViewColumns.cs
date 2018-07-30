using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace LogInspect.Views
{
	public static class GridViewColumns
	{
		private static IndexerBindingConverter converter = new IndexerBindingConverter();

		public static readonly DependencyProperty ColumnsSourceProperty = DependencyProperty.RegisterAttached("ColumnsSource", typeof(object), typeof(GridViewColumns), new UIPropertyMetadata(null, ColumnsSourceChanged));
		public static readonly DependencyProperty HeaderTextMemberProperty = DependencyProperty.RegisterAttached("HeaderTextMember", typeof(string), typeof(GridViewColumns), new UIPropertyMetadata(null));
		public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.RegisterAttached("PropertyName", typeof(string), typeof(GridViewColumns), new UIPropertyMetadata(null));
		public static readonly DependencyProperty CellTemplateProperty = DependencyProperty.RegisterAttached("CellTemplate", typeof(DataTemplate), typeof(GridViewColumns), new UIPropertyMetadata(null));


		[AttachedPropertyBrowsableForType(typeof(GridView))]
		public static object GetColumnsSource(DependencyObject obj)
		{
			return (object)obj.GetValue(ColumnsSourceProperty);
		}
		public static void SetColumnsSource(DependencyObject obj, object value)
		{
			obj.SetValue(ColumnsSourceProperty, value);
		}


		[AttachedPropertyBrowsableForType(typeof(GridView))]
		public static string GetHeaderTextMember(DependencyObject obj)
		{
			return (string)obj.GetValue(HeaderTextMemberProperty);
		}
		public static void SetHeaderTextMember(DependencyObject obj, string value)
		{
			obj.SetValue(HeaderTextMemberProperty, value);
		}


		[AttachedPropertyBrowsableForType(typeof(GridView))]
		public static string GetPropertyName(DependencyObject obj)
		{
			return (string)obj.GetValue(PropertyNameProperty);
		}
		public static void SetPropertyName(DependencyObject obj, string value)
		{
			obj.SetValue(PropertyNameProperty, value);
		}


		[AttachedPropertyBrowsableForType(typeof(GridView))]
		public static DataTemplate GetCellTemplate(DependencyObject obj)
		{
			return (DataTemplate)obj.GetValue(CellTemplateProperty);
		}
		public static void SetCellTemplate(DependencyObject obj, DataTemplate value)
		{
			obj.SetValue(CellTemplateProperty, value);
		}



		private static void ColumnsSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			GridView gridView = obj as GridView;
			if (gridView != null)
			{
				gridView.Columns.Clear();

				if (e.NewValue != null)
				{
					ICollectionView view = CollectionViewSource.GetDefaultView(e.NewValue);
					if (view != null) CreateColumns(gridView, view);
				}
			}
		}

		

		private static void CreateColumns(GridView gridView, ICollectionView view)
		{
			GridViewColumn column;

			column = new GridViewColumn() { Header="#",DisplayMemberBinding=new Binding("Index"),Width=50 };
			gridView.Columns.Add(column);

			foreach (var item in view)
			{
				column = CreateColumn(gridView, item);
				gridView.Columns.Add(column);
			}
		}

		

		private static GridViewColumn CreateColumn(GridView gridView, object columnSource)
		{
			GridViewColumn column = new GridViewColumn();
			string headerTextMember = GetHeaderTextMember(gridView);
			string displayMemberMember = GetPropertyName(gridView);
			if (!string.IsNullOrEmpty(headerTextMember))
			{
				column.Header = GetPropertyValue(columnSource, headerTextMember);
			}
			if (!string.IsNullOrEmpty(displayMemberMember))
			{
				string propertyName = GetPropertyValue(columnSource, displayMemberMember) as string;
				column.DisplayMemberBinding = new Binding() { Converter=converter,ConverterParameter= propertyName };
			}
			
			column.CellTemplate = GetCellTemplate(gridView);
			return column;
		}

		private static object GetPropertyValue(object obj, string propertyName)
		{
			if (obj != null)
			{
				PropertyInfo prop = obj.GetType().GetProperty(propertyName);
				if (prop != null) return prop.GetValue(obj, null);
			}
			return null;
		}



	}
}



