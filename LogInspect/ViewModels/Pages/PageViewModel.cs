using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogLib;

namespace LogInspect.ViewModels.Pages
{
	public abstract class PageViewModel : ViewModel
	{
		public abstract string ImageSource
		{
			get;
		}

		public abstract string Name
		{
			get;
		}

		public PageViewModel(ILogger Logger) : base(Logger)
		{
		}
	}
}
