using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogLib;

namespace LogInspect.ViewModels
{
	public class MarkerViewModel : ViewModel
	{


		public int Position
		{
			get;
			set;
		}

		public int Size
		{
			get;
			set;
		}


		public string Background
		{
			get;
			set;
		}


		public object Severity
		{
			get;
			set;
		}


		public MarkerViewModel(ILogger Logger) : base(Logger)
		{
		}
	}
}
