﻿using LogInspect.ViewModels.Columns;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xml;

namespace LogInspect.Views
{
	public static class GridViewColumns
	{
		public static readonly DependencyProperty ColumnsSourceProperty =	DependencyProperty.RegisterAttached("ColumnsSource",typeof(IEnumerable<ColumnViewModel>),typeof(GridViewColumns),new UIPropertyMetadata(null,ColumnsSourceChanged));

		[AttachedPropertyBrowsableForType(typeof(GridView))]
		public static object GetColumnsSource(DependencyObject obj)
		{
			return (IEnumerable<ColumnViewModel>)obj.GetValue(ColumnsSourceProperty);
		}

		public static void SetColumnsSource(DependencyObject obj, IEnumerable<ColumnViewModel> value)
		{
			obj.SetValue(ColumnsSourceProperty, value);
		}




		


		private static void ColumnsSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			GridView gridView ;
			IEnumerable<ColumnViewModel> columns;

			gridView = sender as GridView;
			if (gridView != null)
			{
				gridView.Columns.Clear();
				columns = e.NewValue as IEnumerable<ColumnViewModel>;
				if (columns != null) CreateColumns(gridView, columns);
			}
		}

		


		private static void CreateColumns(GridView gridView, IEnumerable<ColumnViewModel> Columns)
		{
			foreach (ColumnViewModel item in Columns)
			{
				GridViewColumn column = CreateColumn(gridView, item);
				gridView.Columns.Add(column);
			}
		}

		

		private static GridViewColumn CreateColumn(GridView gridView, ColumnViewModel Column)
		{
			GridViewColumn column = new GridViewColumn();
		
			column.Width = Column.Width;
			column.Header = Column.Name;
			column.CellTemplate = CreateDataTemplate(Column.Name);

			
			//column.DisplayMemberBinding = new Binding($"[{Column.Name}]");


			return column;
		}

		public static DataTemplate CreateDataTemplate(string PropertyName)
		{
			StringReader stringReader = new StringReader(
			@"<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""> 
            <ContentPresenter HorizontalAlignment=""Stretch"" Content=""{Binding [" + PropertyName + @"]}""/> 
			</DataTemplate>");
			XmlReader xmlReader = XmlReader.Create(stringReader);
			return XamlReader.Load(xmlReader) as DataTemplate;
		}

	}
}
