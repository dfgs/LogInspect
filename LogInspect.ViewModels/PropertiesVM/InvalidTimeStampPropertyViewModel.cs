using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInspect.ViewModels.Properties
{
	public class InvalidTimeStampPropertyViewModel : PropertyViewModel
	{


		public InvalidTimeStampPropertyViewModel(ILogger Logger, string Name, string Value) : base(Logger, Name, Value)
		{
		}

	}
}
