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

		private IEnumerable<PropertyViewModel> properties;
		public IEnumerable<PropertyViewModel> Properties
		{
			get { return properties; }
		}
		public PropertiesPageViewModel(ILogger Logger, IEnumerable<PropertyViewModel> Properties) : base(Logger)
		{
			AssertParameterNotNull(Properties,"Properties",  out properties);
		}


	}
}
