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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FormatHandlerDesigner.Views
{
	/// <summary>
	/// Logique d'interaction pour ColumnsView.xaml
	/// </summary>
	public partial class ColumnsView : UserControl
	{
		public ColumnsView()
		{
			InitializeComponent();
		}

		private void ColumnListView_AddItem(object sender, AddItemEventArgs e)
		{
			e.AddedItem = new Column() { Name = "New column" };
		}


	}
}
