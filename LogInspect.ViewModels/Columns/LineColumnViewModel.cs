﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogInspect.Models.Filters;
using LogInspect.ViewModels.Filters;
using LogInspect.ViewModels.Properties;
using LogInspect.Models;
using LogInspect.Models.Parsers;
using LogLib;

namespace LogInspect.ViewModels.Columns
{
	public class LineColumnViewModel : ColumnViewModel
	{
		public override bool AllowsResize => false;
		public override bool AllowsFilter => false;

		public override bool IsImageVisible => true;
		public override string ImageSource => "/LogInspect;component/Images/text_indent.png";


		public LineColumnViewModel(ILogger Logger,string Name) : base(Logger,Name,"","Right")
		{
		}

		public override PropertyViewModel CreatePropertyViewModel(Event Event)
		{
			return new LinePropertyViewModel(Logger,Name, Event.LineIndex+1);
		}

		public override FilterViewModel CreateFilterViewModel()
		{
			return null;
		}


	}
}
