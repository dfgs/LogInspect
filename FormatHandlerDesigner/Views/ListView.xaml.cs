using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FormatHandlerDesigner.Views
{
	/// <summary>
	/// Logique d'interaction pour ListView.xaml
	/// </summary>
	public partial class ListView : UserControl
	{

		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(ListView));
		public string Header
		{
			get { return (string)GetValue(HeaderProperty); }
			set { SetValue(HeaderProperty, value); }
		}

		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(ListViewModel), typeof(ListView));
		public ListViewModel ItemsSource
		{
			get { return (ListViewModel)GetValue(ItemsSourceProperty); }
			private set { SetValue(ItemsSourceProperty, value); }
		}


		public static readonly DependencyProperty ListProperty = DependencyProperty.Register("List", typeof(IList), typeof(ListView),new PropertyMetadata(null,ListPropertyChanged));
		public IList List
		{
			get { return (IList)GetValue(ListProperty); }
			set { SetValue(ListProperty, value); }
		}


		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(ViewModel), typeof(ListView));
		public ViewModel SelectedItem
		{
			get { return (ViewModel)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}


		public event AddItemEventHandler AddItem;

		public ListView()
		{
			InitializeComponent();
		}


		private static void ListPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ListView)d).OnListChanged();
		}
		protected virtual void OnListChanged()
		{
			if (List == null) ItemsSource = null;
			else ItemsSource = new ListViewModel(List);
		}


		private void AddCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (ItemsSource != null) && (AddItem!=null);e.Handled = true;
		}

		private void AddCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			AddItemEventArgs e2;
			e2 = new AddItemEventArgs();

			AddItem?.Invoke(this, e2);
			if (e2.AddedItem!=null)
			{
				SelectedItem = ItemsSource.Add(e2.AddedItem);
			}
			
		}

		private void DeleteCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (ItemsSource!=null) && (SelectedItem!=null); e.Handled = true;
		}

		private void DeleteCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ItemsSource.RemoveAt(ItemsSource.IndexOf(SelectedItem));
		}


	}
}
