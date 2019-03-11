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
using System.Xml;

namespace LogInspect.Views
{
	/// <summary>
	/// Logique d'interaction pour XmlViewer.xaml
	/// </summary>
	public partial class XmlViewer : UserControl
	{


		public static readonly DependencyProperty XmlDocumentProperty = DependencyProperty.Register("XmlDocument", typeof(XmlDocument), typeof(XmlViewer),new FrameworkPropertyMetadata(null,XmlDocumentPropertyChanged));
		public XmlDocument XmlDocument
		{
			get { return (XmlDocument)GetValue(XmlDocumentProperty); }
			set { SetValue(XmlDocumentProperty, value); }
		}

		

		public XmlViewer()
		{
			InitializeComponent();
		}

		private static void XmlDocumentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((XmlViewer)d).OnXmlDocumentChanged();
		}


		protected virtual void OnXmlDocumentChanged()
		{
			BindXMLDocument();
		}

		private void BindXMLDocument()
		{
			if (XmlDocument == null)
			{
				xmlTree.ItemsSource = null;
				return;
			}

			XmlDataProvider provider = new XmlDataProvider();
			provider.Document = XmlDocument;
			Binding binding = new Binding();
			binding.Source = provider;
			binding.XPath = "child::node()";
			xmlTree.SetBinding(TreeView.ItemsSourceProperty, binding);
		}
	}
}
