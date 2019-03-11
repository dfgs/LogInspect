using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogInspect.ViewModels.Properties;
using LogLib;

namespace LogInspect.ViewModels.Pages
{
	public class PropertiesPageViewModel : PageViewModel
	{
		public override string ImageSource => "/LogInspect;component/Images/information.png";

		public override string Name => "Properties";


		public IEnumerable<PropertyViewModel> Properties
		{
			get;
			private set;
		}
		public PropertiesPageViewModel(ILogger Logger, IEnumerable<PropertyViewModel> Properties) : base(Logger)
		{
			AssertParameterNotNull("Properties", Properties);
			this.Properties = Properties;
		}


	}
}
