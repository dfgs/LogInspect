﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogInspect.ViewModels.Filters
{
	public class FilterItemViewModel :INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private bool isChecked;
		public bool IsChecked
		{
			get { return isChecked; }
			set { isChecked = value;OnPropertyChanged(); }
		}

		private object value;
		public object Value
		{
			get { return value; }
			set { this.value = value;OnPropertyChanged(); }
		}

		protected virtual void OnPropertyChanged([CallerMemberName]string PropertyName=null)
		{
			if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
		}
		
	}
}
