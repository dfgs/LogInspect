using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.ViewModels.Columns;
using LogInspect.Models;
using LogLib;

namespace LogInspect.ViewModels.Properties
{
	public abstract class PropertyViewModel : ViewModel
	{
		public string Name
		{
			get;
			private set;
		}

		private object value;
		public object Value
		{
			get { return value; }
			set { this.value = value; OnPropertyChanged(); }
		}

		
		public PropertyViewModel(ILogger Logger,string Name,object Value) : base(Logger)
		{
			this.Name = Name;this.Value = Value;
		}

		public override string ToString()
		{
			return value?.ToString()??"";
		}

	}
}
