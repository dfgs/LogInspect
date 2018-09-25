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
	/// Logique d'interaction pour ProgressView.xaml
	/// </summary>
	public partial class ProgressView : UserControl
	{

		public static readonly DependencyProperty StreamValueProperty = DependencyProperty.Register("StreamValue", typeof(double), typeof(ProgressView));
		public double StreamValue
		{
			get { return (double)GetValue(StreamValueProperty); }
			set { SetValue(StreamValueProperty, value); }
		}


		public static readonly DependencyProperty StreamMaximumProperty = DependencyProperty.Register("StreamMaximum", typeof(double), typeof(ProgressView));
		public double StreamMaximum
		{
			get { return (double)GetValue(StreamMaximumProperty); }
			set { SetValue(StreamMaximumProperty, value); }
		}



		

		public ProgressView()
		{
			InitializeComponent();
		}
	}
}
