using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Linq;
using System.Reflection;

namespace FormatHandlerDesigner.Views
{
	/// <summary>
	/// Logique d'interaction pour PropertyView.xaml
	/// </summary>
	public partial class PropertyView : UserControl
	{
		public static string[] ColorsItemsSource;

		public static readonly DependencyProperty PropertyTypeProperty = DependencyProperty.Register("PropertyType", typeof(PropertyTypes), typeof(PropertyView));
		public PropertyTypes PropertyType
		{
			get { return (PropertyTypes)GetValue(PropertyTypeProperty); }
			set { SetValue(PropertyTypeProperty, value); }
		}

		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(PropertyView));
		public string Header
		{
			get { return (string)GetValue(HeaderProperty); }
			set { SetValue(HeaderProperty, value); }
		}


		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(PropertyView),new FrameworkPropertyMetadata(null,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
		public object Value
		{
			get { return GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		static PropertyView()
		{
			List<string> items;
			items = new List<string>();
			foreach (PropertyInfo pi in typeof(Colors).GetProperties(BindingFlags.Static | BindingFlags.Public) )
			{
				items.Add(pi.Name);
			}
			ColorsItemsSource = items.ToArray();
		}

		public PropertyView()
		{
			InitializeComponent();
			
		}
	}
}
