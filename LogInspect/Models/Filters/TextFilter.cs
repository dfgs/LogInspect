﻿using LogInspectLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogInspect.Models.Filters
{
    public class TextFilter:Filter
    {

		public string Value
		{
			get;
			set;
		}

	

		public override bool MustDiscard(Event Item)
		{
			return false;// Item.Severity != Value;
		}

	}
}
