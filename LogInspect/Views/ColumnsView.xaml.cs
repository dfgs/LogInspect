using LogInspect.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Logique d'interaction pour ColumnsView.xaml
    /// </summary>
    public partial class ColumnsView : UserControl
    {
		public static readonly ColumnViewModel[] TestColumns = new ColumnViewModel[] { new PropertyColumnViewModel(null, "Column 1",null), new PropertyColumnViewModel(null, "Column 2", null), new PropertyColumnViewModel(null, "Column 3", null), new PropertyColumnViewModel(null, "Column 4", null), new PropertyColumnViewModel(null, "Column 5", null) };


		public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register("HorizontalOffset", typeof(double), typeof(ColumnsView));
		public double HorizontalOffset
		{
			get { return (double)GetValue(HorizontalOffsetProperty); }
			set { SetValue(HorizontalOffsetProperty, value); }
		}


		


		public ColumnsView()
        {
            InitializeComponent();
        }

		private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
		{
			Thumb thumb;
			FrameworkElement parent;
			double newWidth;

			thumb = sender as Thumb;
			parent = thumb.Parent as FrameworkElement;

			newWidth = parent.Width + e.HorizontalChange;
			if (newWidth < 1) newWidth = 1;
			parent.Width = newWidth ;
		}
	}
}
