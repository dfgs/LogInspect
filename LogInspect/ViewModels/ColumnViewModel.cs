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
		public string Name
		{
			get;
			private set;
		}

		public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(double), typeof(ColumnViewModel),new PropertyMetadata(100d));
		public double Width
		{
			get { return (double)GetValue(WidthProperty); }
			set { SetValue(WidthProperty, value); }
		}

		public ColumnViewModel(ILogger Logger,string Name) : base(Logger)
		{
			this.Name = Name;
		}

	}
}
