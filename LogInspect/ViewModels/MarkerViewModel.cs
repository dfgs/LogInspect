using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LogLib;

namespace LogInspect.ViewModels
{
	public class MarkerViewModel : ViewModel
	{


		public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(int), typeof(MarkerViewModel));
		public int Position
		{
			get { return (int)GetValue(PositionProperty); }
			set { SetValue(PositionProperty, value); }
		}

		public static readonly DependencyProperty SizeProperty = DependencyProperty.Register("Size", typeof(int), typeof(MarkerViewModel));
		public int Size
		{
			get { return (int)GetValue(SizeProperty); }
			set { SetValue(SizeProperty, value); }
		}


		public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(Brush), typeof(MarkerViewModel));
		public Brush Background
		{
			get { return (Brush)GetValue(BackgroundProperty); }
			set { SetValue(BackgroundProperty, value); }
		}


		public static readonly DependencyProperty SeverityProperty = DependencyProperty.Register("Severity", typeof(string), typeof(MarkerViewModel));
		public string Severity
		{
			get { return (string)GetValue(SeverityProperty); }
			set { SetValue(SeverityProperty, value); }
		}


		public MarkerViewModel(ILogger Logger) : base(Logger,-1)
		{
		}
	}
}
