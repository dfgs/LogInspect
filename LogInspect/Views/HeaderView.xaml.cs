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
	/// Logique d'interaction pour HeaderView.xaml
	/// </summary>
	public partial class HeaderView : UserControl
	{
		public static readonly string[] TestHeaders = new string[] {"Column1", "Column2", "Column3", "Column4", "Column5" };
		public HeaderView()
		{
			InitializeComponent();
		}

		private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
		{
			Thumb thumb;
			ColumnViewModel column;

			thumb = (Thumb)sender;
			column = thumb.DataContext as ColumnViewModel;
			if (column == null) return;
			column.Width += e.HorizontalChange;
		
		}
	}
}
