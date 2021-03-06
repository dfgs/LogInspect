﻿using LogInspect.ViewModels.Columns;
using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace LogInspect.ViewModels
{
	public class FindOptions : ViewModel
    {
		private bool isVisible;
		public bool IsVisible
		{
			get { return isVisible; }
			set { isVisible = value;OnPropertyChanged(); }
		}

		public string Text
		{
			get;
			set;
		}


		public bool MatchWholeWord
		{
			get;
			set;
		}


		public bool CaseSensitive
		{
			get;
			set;
		}


		public string Column
		{
			get;
			set;
		}
		public FindOptions(ILogger Logger) : base(Logger)
		{
		}

		public bool Match(EventViewModel Event)
		{
			string pattern;
			
			if (MatchWholeWord) pattern = $@"\b{Regex.Escape(Text)}\b";
			else pattern = $@"{Regex.Escape(Text)}";

			return Regex.Match(Event[Column].ToString(),pattern,CaseSensitive?RegexOptions.None: RegexOptions.IgnoreCase).Success;
		}
		

	}
}
