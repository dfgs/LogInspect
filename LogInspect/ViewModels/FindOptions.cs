using LogInspect.ViewModels.Columns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace LogInspect.ViewModels
{
    public class FindOptions:DependencyObject
    {
		public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.Register("IsVisible", typeof(bool), typeof(FindOptions));
		public bool IsVisible
		{
			get { return (bool)GetValue(IsVisibleProperty); }
			set { SetValue(IsVisibleProperty, value); }
		}

		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(FindOptions));
		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}


		public static readonly DependencyProperty MatchWholeWordProperty = DependencyProperty.Register("MatchWholeWord", typeof(bool), typeof(FindOptions));
		public bool MatchWholeWord
		{
			get { return (bool)GetValue(MatchWholeWordProperty); }
			set { SetValue(MatchWholeWordProperty, value); }
		}


		public static readonly DependencyProperty CaseSensitiveProperty = DependencyProperty.Register("CaseSensitive", typeof(bool), typeof(FindOptions));
		public bool CaseSensitive
		{
			get { return (bool)GetValue(CaseSensitiveProperty); }
			set { SetValue(CaseSensitiveProperty, value); }
		}


		public static readonly DependencyProperty ColumnProperty = DependencyProperty.Register("Column", typeof(string), typeof(FindOptions));
		public string Column
		{
			get { return (string)GetValue(ColumnProperty); }
			set { SetValue(ColumnProperty, value); }
		}


		public bool Match(EventViewModel Event)
		{
			string pattern;
			
			if (MatchWholeWord) pattern = $@"\b{Regex.Escape(Text)}\b";
			else pattern = $@"{Regex.Escape(Text)}";

			return Regex.Match(Event.GetPropertyValue(Column)?.ToString()??"",pattern,CaseSensitive?RegexOptions.None: RegexOptions.IgnoreCase).Success;
		}
		

	}
}
