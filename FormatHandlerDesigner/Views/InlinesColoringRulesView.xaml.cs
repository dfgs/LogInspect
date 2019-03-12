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
    /// Logique d'interaction pour InlinesColoringRulesView.xaml
    /// </summary>
    public partial class InlinesColoringRulesView : UserControl
    {
        public InlinesColoringRulesView()
        {
            InitializeComponent();
        }

		private void list_AddItem(object sender, AddItemEventArgs e)
		{
			//e.AddedItem=new InlineColoringRule() { PatternName=".*" };
		}
	}
}
