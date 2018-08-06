using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogInspect.ViewModels
{
	public class SeverityViewModel:ViewModel
	{
		public string Name
		{
			get;
			private set;
		}

		public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(SeverityViewModel),new PropertyMetadata(true));
		public bool IsChecked
		{
			get { return (bool)GetValue(IsCheckedProperty); }
			set { SetValue(IsCheckedProperty, value); }
		}

		public SeverityViewModel(ILogger Logger,string Name):base(Logger)
		{
			this.Name = Name;
		}
	}
}
