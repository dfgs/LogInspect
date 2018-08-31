using LogInspectLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FormatHandlerDesigner
{
	public class AppViewModel:DependencyObject
	{

		public ObservableCollection<FormatHandler> ItemsSource
		{
			get;
			private set;
		}
		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(FormatHandler), typeof(AppViewModel));
		public FormatHandler SelectedItem
		{
			get { return (FormatHandler)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

	
		public AppViewModel() : base()
		{
			ItemsSource = new ObservableCollection<FormatHandler>();
		}

		

		public void Open(string FileName)
		{
			FormatHandler schema;

			schema = FormatHandler.LoadFromFile(FileName);
			ItemsSource.Add(schema);
			SelectedItem = schema;
		}
		public void CreateNew()
		{
			FormatHandler schema;

			schema = new FormatHandler() {Name ="New format handler" };
			ItemsSource.Add(schema);
			SelectedItem = schema;
		}
		public void CloseCurrent()
		{
			if (SelectedItem == null) return;
			ItemsSource.Remove(SelectedItem);
			SelectedItem = ItemsSource.FirstOrDefault();
		}

		
	}

}
