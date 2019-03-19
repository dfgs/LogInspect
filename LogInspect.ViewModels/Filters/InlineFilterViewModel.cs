using LogInspect.Models;
using LogInspect.ViewModels.Properties;
using LogLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogInspect.ViewModels.Filters
{
	public class InlineFilterViewModel:TextFilterViewModel
	{

		

		public InlineFilterViewModel(ILogger Logger, string PropertyName, TextFilterViewModel Model):base(Logger,PropertyName,Model)
		{
			
		}

		
		



	}
}
