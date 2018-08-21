using LogInspect.ViewModels;
using LogInspect.ViewModels.Columns;
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
		public static readonly ColumnViewModel[] TestColumns = new ColumnViewModel[] { new TextPropertyColumnViewModel(null, "Column 1",null), new TextPropertyColumnViewModel(null, "Column 2", null), new TextPropertyColumnViewModel(null, "Column 3", null), new TextPropertyColumnViewModel(null, "Column 4", null), new TextPropertyColumnViewModel(null, "Column 5", null) };


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

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Button button;
			ColumnViewModel column;
			FilterWindow window;
			bool? result;

			button = sender as Button;
			if (button == null) return;
			column = button.DataContext as ColumnViewModel;
			if (column == null) return;

			window = new FilterWindow();window.Owner = Application.Current.MainWindow;window.Filter = column.CreateFilterViewModel();
			result = window.ShowDialog();
			if (result == true) column.Filter = window.Filter.CreateFilter();
			else column.Filter = null;

			
		}


	}
}
