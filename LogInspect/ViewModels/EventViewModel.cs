using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LogInspect.ViewModels.Columns;
using LogInspect.ViewModels.Properties;
using LogInspectLib;
using LogInspectLib.Parsers;
using LogLib;

namespace LogInspect.ViewModels
{
	public class EventViewModel : ViewModel
	{

		private Event ev;

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
		
		public int EventIndex
		{
			get;
			set;
		}

		public int LineIndex
		{
			get { return ev.LineIndex; }
		}


		private TimeStampPropertyViewModel timeStamp;
		public DateTime TimeStamp
		{
			get
			{
				return (DateTime)(timeStamp?.Value??DateTime.MinValue);
			}
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
		}

		
		public string Lines
		{
			get
			{
				return "TODO";
			}
		}


		public bool IsBookMarked
		{
			get { return this.ev.IsBookMarked; }
			set { this.ev.IsBookMarked=value;OnPropertyChanged(); }
		}



		public EventViewModel(ILogger Logger, IEnumerable<ColumnViewModel> Columns,  IEnumerable<EventColoringRule> ColoringRules, Event Event) : base(Logger,-1)
		{
			//string severity;
			
			this.ev = Event;

			Background = GetBackground(ColoringRules,Event );
			if (Background == null)
			{
				Background = Brushes.LightGray;
				Foreground = Brushes.Black;
			}
			else Foreground = Background;

			properties = new PropertyCollection<PropertyViewModel>();
			foreach(ColumnViewModel column in Columns)
			{
				properties[column.Name] = column.CreatePropertyViewModel(this);
			}
			timeStamp = properties.FirstOrDefault(item => item is TimeStampPropertyViewModel) as TimeStampPropertyViewModel;
		}

		public static Brush GetBackground(IEnumerable<EventColoringRule> ColoringRules,Event Event)
		{
			string value;

			foreach (EventColoringRule coloringRule in ColoringRules)
			{
				value = Event[coloringRule.Column];
				if (value == null) continue;

				if (Regex.Match(value, coloringRule.Pattern).Success)
				{
					try
					{
						Brush Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(coloringRule.Background));
						return Background;
					}
					catch 
					{
						//Log(LogLevels.Error, $"Invalid background {coloringRule.Background}");
					}

				}
			}
			return null;
		}

		public static Brush GetBackground(IEnumerable<EventColoringRule> ColoringRules, EventViewModel Event)
		{
			string value;

			foreach (EventColoringRule coloringRule in ColoringRules)
			{
				value = Event.GetEventValue( coloringRule.Column);
				if (value == null) continue;

				if (Regex.Match(value, coloringRule.Pattern).Success)
				{
					try
					{
						Brush Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(coloringRule.Background));
						return Background;
					}
					catch
					{
						//Log(LogLevels.Error, $"Invalid background {coloringRule.Background}");
					}

				}
			}
			return null;
		}


		public string GetEventValue(string Column)
		{
			return ev[Column];
		}
		/*public Inline[] GetPropertyInlines(string PropertyName)
		{
			return ev.GetProperty(PropertyName)?.Inlines;
		}*/
		

	}

}
