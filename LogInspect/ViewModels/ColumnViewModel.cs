using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogLib;

namespace LogInspect.ViewModels
{
	public class ColumnViewModel : ViewModel
	{
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(ColumnViewModel));
		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}
		public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(double), typeof(ColumnViewModel));
		public double Width
		{
			get { return (double)GetValue(WidthProperty); }
			set { SetValue(WidthProperty, value); }
		}

		public ColumnViewModel(ILogger Logger,string Text=null,double Width=100d) : base(Logger)
		{
			this.Text = Text;this.Width = Width;
		}


	}
}
