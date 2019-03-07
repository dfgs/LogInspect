﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.ViewModels.Columns;
using LogInspectLib;
using LogInspectLib.Parsers;
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
		

		public  object Value
		{
			get;
			private set;
		}

		
		public PropertyViewModel(ILogger Logger,string Name,object Value) : base(Logger)
		{
			this.Name = Name;this.Value = Value;
		}


	}
}
