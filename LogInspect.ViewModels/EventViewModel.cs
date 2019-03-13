using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using LogInspect.Modules;
using LogInspect.ViewModels.Columns;
using LogInspect.ViewModels.Pages;
using LogInspect.ViewModels.Properties;
using LogInspect.Models;
using LogLib;

namespace LogInspect.ViewModels
{
	public class EventViewModel : ViewModel
	{

		//private readonly Event ev;

		private PropertyCollection<PropertyViewModel> properties;
		public IEnumerable<PropertyViewModel> Properties
		{
			get { return properties; }
		}

		public PagesViewModel Pages
		{
			get;
			private set;
		}

		public PropertyViewModel this[string Name]
		{
			get
			{
				return properties[Name];
			}
		}
		
	

		private TimeStampPropertyViewModel timeStamp;
		public DateTime TimeStamp
		{
			get
			{
				return (DateTime)(timeStamp.Value);
			}
		}

		public string Brush
		{
			get;
			private set;
		}
		
		
		public string Lines
		{
			get
			{
				return "TODO";
			}
		}


	


		public EventViewModel(ILogger Logger,string Brush,  IEnumerable<PropertyViewModel> Properties) : base(Logger)
		{
			AssertParameterNotNull(Properties,"Properties");

			this.Brush = Brush;
			properties = new PropertyCollection<PropertyViewModel>();
			foreach(PropertyViewModel property in Properties)
			{
				properties[property.Name] = property;
			}
			
			timeStamp = properties.FirstOrDefault(item => item is TimeStampPropertyViewModel) as TimeStampPropertyViewModel;
			if (timeStamp == null) timeStamp = new TimeStampPropertyViewModel(Logger, "Date",  DateTime.MinValue);

			Pages = new PagesViewModel(Logger);
			Pages.Add(new PropertiesPageViewModel(Logger,properties));
			Pages.AddRange(Properties.OfType<InlinePropertyViewModel>().SelectMany(item => item.Documents).Select(item => new XmlPageViewModel(Logger, item)));
			Pages.SelectedItem = Pages.FirstOrDefault();
		}





		

		

	}

}
