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
	/// Logique d'interaction pour EventView.xaml
	/// </summary>
	public partial class EventView : UserControl
	{
		private LogFileViewModel ViewModel;

		public EventView()
		{
			InitializeComponent();
			DataContextChanged += EventView_DataContextChanged;
		}

		private void EventView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			ViewModel = DataContext as LogFileViewModel;
		}

		private void ListView_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			e.Handled = true;
			ViewModel.Position -= e.Delta;
		}

		private void ListView_PreviewKeyDown(object sender, KeyEventArgs e)
		{

			switch(e.Key)
			{
				case Key.Up:
					e.Handled = true;
					 ViewModel.Position--;
					break;
				case Key.Down:
					e.Handled = true;
					ViewModel.Position++;
					break;
				default:
					e.Handled = false;
					break;
			}

		}


	}
}
