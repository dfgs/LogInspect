using LogInspect.Models;
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
using System.Windows.Shapes;

namespace LogInspect
{
	/// <summary>
	/// Logique d'interaction pour FormatHandlerSelectionWindow.xaml
	/// </summary>
	public partial class FormatHandlerSelectionWindow : Window
	{

		public static readonly DependencyProperty FormatHandlersProperty = DependencyProperty.Register("FormatHandlers", typeof(IEnumerable<FormatHandler>), typeof(FormatHandlerSelectionWindow));
		public IEnumerable<FormatHandler> FormatHandlers
		{
			get { return (IEnumerable<FormatHandler>)GetValue(FormatHandlersProperty); }
			set { SetValue(FormatHandlersProperty, value); }
		}

		public static readonly DependencyProperty SelectedFormatHandlerProperty = DependencyProperty.Register("SelectedFormatHandler", typeof(FormatHandler), typeof(FormatHandlerSelectionWindow));
		public FormatHandler SelectedFormatHandler
		{
			get { return (FormatHandler)GetValue(SelectedFormatHandlerProperty); }
			set { SetValue(SelectedFormatHandlerProperty, value); }
		}

		public FormatHandlerSelectionWindow()
		{
			InitializeComponent();
		}

		private void ButtonOK_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}
		
		private void ButtonCancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}




	}
}
