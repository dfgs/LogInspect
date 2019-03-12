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
    /// Logique d'interaction pour TokensView.xaml
    /// </summary>
    public partial class TokensView : UserControl
    {

		public static readonly DependencyProperty RulePatternProperty = DependencyProperty.Register("RulePattern", typeof(string), typeof(TokensView));
		public string RulePattern
		{
			get { return (string)GetValue(RulePatternProperty); }
			set { SetValue(RulePatternProperty, value); }
		}

		public TokensView()
        {
            InitializeComponent();
        }

		private void list_AddItem(object sender, AddItemEventArgs e)
		{
			e.AddedItem = new Token() { Pattern = ".*" };
		}

		private void PatternTestView_GetPattern(object sender, GetPatternEventArgs e)
		{
			Rule rule;

			rule = DataContext as Rule;
			if (rule == null) return;
			e.Pattern = rule.GetPattern();
			this.RulePattern = e.Pattern;
		}
	}
}
