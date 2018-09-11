using LogInspect.ViewModels;
using System;
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

namespace LogInspect.Views
{
	/// <summary>
	/// Logique d'interaction pour FindView.xaml
	/// </summary>
	public partial class FindView : UserControl
	{

		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(FindView));
		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}


		public static readonly DependencyProperty MatchWholeWordProperty = DependencyProperty.Register("MatchWholeWord", typeof(bool), typeof(FindView));
		public bool MatchWholeWord
		{
			get { return (bool)GetValue(MatchWholeWordProperty); }
			set { SetValue(MatchWholeWordProperty, value); }
		}


		public static readonly DependencyProperty CaseSensitiveProperty = DependencyProperty.Register("CaseSensitive", typeof(bool), typeof(FindView));
		public bool CaseSensitive
		{
			get { return (bool)GetValue(CaseSensitiveProperty); }
			set { SetValue(CaseSensitiveProperty, value); }
		}


		public FindView()
		{
			InitializeComponent();
		}


		#region commands
		private void CloseCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			LogFileViewModel vm;
			vm = DataContext as LogFileViewModel;
			e.CanExecute = (vm != null) ; e.Handled = true;
		}

		private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			LogFileViewModel vm;
			vm = DataContext as LogFileViewModel;

			vm.IsFindMenuVisible = false;
		}

		private void FindPreviousCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			LogFileViewModel vm;
			vm = DataContext as LogFileViewModel;
			e.CanExecute = (vm != null) && (vm.Columns.SelectedItem!=null) && (vm.Status == Statuses.Idle) && (!string.IsNullOrEmpty(Text)); e.Handled = true;
		}

		private async void FindPreviousCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			int index;
			LogFileViewModel vm;
			vm = DataContext as LogFileViewModel;

			index = await vm.FindPreviousAsync(vm.Events.SelectedItem?.EventIndex ?? -1,(item)=>item.GetPropertyValue(vm.Columns.SelectedItem.Name)?.ToString()?.Contains(Text)??false) ;
			if (index >= 0) vm.Events.Select(index);
		}

		private void FindNextCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			LogFileViewModel vm;
			vm = DataContext as LogFileViewModel;
			e.CanExecute = (vm != null) && (vm.Columns.SelectedItem != null) && (vm.Status == Statuses.Idle) && (!string.IsNullOrEmpty(Text)); e.Handled = true;
		}

		private async void FindNextCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			int index;
			LogFileViewModel vm;
			vm = DataContext as LogFileViewModel;

			index = await vm.FindNextAsync( vm.Events.SelectedItem?.EventIndex ?? -1, (item) => item.GetPropertyValue(vm.Columns.SelectedItem.Name)?.ToString()?.Contains(Text) ?? false);
			if (index >= 0) vm.Events.Select(index);
		}
		#endregion


	}
}
