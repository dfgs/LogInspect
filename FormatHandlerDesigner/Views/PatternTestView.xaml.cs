﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
	/// Logique d'interaction pour PatternTestView.xaml
	/// </summary>
	public partial class PatternTestView : UserControl
	{


		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(PatternTestView));
		public Orientation Orientation
		{
			get { return (Orientation)GetValue(OrientationProperty); }
			set { SetValue(OrientationProperty, value); }
		}

		public static readonly DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(bool?), typeof(PatternTestView));
		public bool? Status
		{
			get { return (bool?)GetValue(StatusProperty); }
			set { SetValue(StatusProperty, value); }
		}


		public static readonly DependencyProperty PatternProperty = DependencyProperty.Register("Pattern", typeof(string), typeof(PatternTestView));
		public string Pattern
		{
			get { return (string)GetValue(PatternProperty); }
			set { SetValue(PatternProperty, value); }
		}

		public PatternTestView()
		{
			InitializeComponent();
		}

		private void TestCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (!string.IsNullOrEmpty(Pattern)) && (!string.IsNullOrEmpty(textBox.Text));e.Handled = true;
		}

		private void TestCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Status=Regex.Match(textBox.Text, Pattern).Success;

		}

		private void textBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			Status = null;
		}
	}
}
