﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.ViewModels.Columns;
using LogLib;

namespace LogInspect.ViewModels.Properties
{
	public class BookMarkPropertyViewModel : PropertyViewModel
	{
		

		
		public BookMarkPropertyViewModel(ILogger Logger, string Name, TextAlignment Alignment, EventViewModel Event) : base(Logger,Name,Alignment)
		{
			this.Value = Event;
		}
	}
}
