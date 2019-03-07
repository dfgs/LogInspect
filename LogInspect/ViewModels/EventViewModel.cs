using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LogInspect.Modules;
using LogInspect.ViewModels.Columns;
using LogInspect.ViewModels.Properties;
using LogInspectLib;
using LogInspectLib.Parsers;
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

		public PropertyViewModel this[string Name]
		{
			get
			{
				return properties[Name];
			}
		}
		
		
		/*public int LineIndex
		{
			get;
			private set;
		}*/


		private TimeStampPropertyViewModel timeStamp;
		public DateTime TimeStamp
		{
			get
			{
				return (DateTime)(timeStamp.Value);
			}
		}

		/*public Brush SeverityBrush
		{
			get;
			private set;
		}
		public Brush Background
		{
			get;
			private set;
		}
		public Brush Foreground
		{
			get;
			private set;
		}
		public string RawLog
		{
			get { return "TODO"; }
		}*/

		
		public string Lines
		{
			get
			{
				return "TODO";
			}
		}


		/*private bool isBookMarked;
		public bool IsBookMarked
		{
			get { return isBookMarked; }
			set { isBookMarked=value;OnPropertyChanged(); }
		}*/



		public EventViewModel(ILogger Logger,  IEnumerable<PropertyViewModel> Properties) : base(Logger)
		{
			AssertParameterNotNull("Properties", Properties);

			/*this.LineIndex = LineIndex;
			this.SeverityBrush = SeverityBrush;

			if (SeverityBrush == null)
			{
				Background = Brushes.LightGray;
				Foreground = Brushes.Black;
			}
			else
			{
				Background = SeverityBrush;
				Foreground = SeverityBrush;
			}*/

			properties = new PropertyCollection<PropertyViewModel>();
			foreach(PropertyViewModel property in Properties)
			{
				properties[property.Name] = property;
			}
			
			timeStamp = properties.FirstOrDefault(item => item is TimeStampPropertyViewModel) as TimeStampPropertyViewModel;
			if (timeStamp == null) timeStamp = new TimeStampPropertyViewModel(Logger, "Date",  DateTime.MinValue);
		}




		/*public string GetEventValue(string Column)
		{
			return ev[Column];
		}*/
		

		

	}

}
